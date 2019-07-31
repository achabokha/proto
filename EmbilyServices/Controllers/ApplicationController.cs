using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AutoMapper;
using Embily.Messages;
using Embily.Models;
using EmbilyServices.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Embily.Gateways;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using Embily.Services;

namespace EmbilyServices.Controllers
{
    public class ApplicationMapProfile : Profile
    {
        public ApplicationMapProfile()
        {
            CreateMap<Application, PassportViewModel>();
            CreateMap<PassportViewModel, Application>();
        }
    }

    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ApplicationController : BaseController
    {
        readonly IHostingEnvironment _env;
        readonly EmbilyDbContext _ctx;
        readonly IMapper _mapper;
        readonly IEmailQueueSender _emailSender;
        readonly ICardOrder _cardOrder;
        readonly IShippingCalc _shippingCalc;
        readonly IRefGen _refGen;

        public ApplicationController(
            IHostingEnvironment env,
            EmbilyDbContext ctx,
            IEmailQueueSender emailSender,
            IMapper mapper,
            ICardOrder cardOrder,
            IShippingCalc shippingCalc,
            IRefGen refGen
            )
        {
            _env = env;
            _ctx = ctx;
            _mapper = mapper;
            _emailSender = emailSender;
            _cardOrder = cardOrder;
            _shippingCalc = shippingCalc;
            _refGen = refGen;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> StartApplication([FromBody] dynamic model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currencyCode = ((string)model.CurrencyCode).ParseEnum<CurrencyCodes>();

            string userId = GetUserId();

            // card type should come from UI, aka user choice --
            var provider = _env.IsProduction() ? AccountProviders.KoKard : AccountProviders.Virtual;

            var applicationId = await CreateNewApplication(userId, currencyCode, provider);

            return Ok(new { Status = "success", Message = $"{applicationId} started successfully", applicationId });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateApplication([FromBody] PassportViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var app = await _ctx.Applications.FindAsync(model.ApplicationId);

            //Mapper.Initialize(cfg => cfg.CreateMap<PassportViewModel, Application>()
            //    .ForMember(a => a.DateOfBirth, opt => opt.MapFrom(p => p.DateOfBirth.Year + "-" + p.DateOfBirth.Month + "-" + p.DateOfBirth.Day)));
            //app = Mapper.Map<PassportViewModel, Application>(model);

            app = _mapper.Map(model, app);
            app.DateOfBirth = GetDateOfBirthToString(model.DateOfBirth);

            await _ctx.SaveChangesAsync();

            return Ok(new { Status = "success", Message = $"{app.ApplicationId} started successfully", ApplicationId = app.ApplicationId });
        }
        private string GetDateOfBirthToString(PassportDateOfBirth passportDateOfBirth)
        {
            return $"{passportDateOfBirth.year}-{passportDateOfBirth.month}-{passportDateOfBirth.day}";
        }

        [HttpGet("[action]/{appId}")]
        public async Task<string> GetComments(string appId)
        {
            var app = await _ctx.Applications.FindAsync(appId);
            return app.Comments;
        }

        [HttpGet("[action]/{appId}")]
        public async Task<PassportViewModel> GetPassportInfo(string appId)
        {
            var app = await _ctx.Applications.FindAsync(appId);
            PassportViewModel passport = new PassportViewModel();

            //var passport = _mapper.Map<PassportViewModel>(app);
            passport.ApplicationId = app.ApplicationId;          
            passport.FirstName = app.FirstName;
            passport.LastName = app.LastName;
            passport.Gender = app.GenderString;
            passport.Nationality = app.Nationality;
            passport.Title = app.TitleString;
            passport.MaritalStatus = app.MaritalStatusString;
            passport.Email = app.Email;
            passport.Phone = app.Phone;
            passport.DateOfBirth = new PassportDateOfBirth(app.DateOfBirth);

            return passport;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Submit([FromBody] dynamic model)
        {
            var application = await _ctx.Applications.FindAsync((string)model.ApplicationId);

            application.Status = ApplicationStatus.Submitted;
            await _ctx.SaveChangesAsync();

            var user = await _ctx.Users.FindAsync(this.GetUserId());
            await _emailSender.ApplicationSubmittedAsync(user, application);
            await _emailSender.NewApplicationAlertAsync(user, application);

            return Ok(new { Status = "success", Message = $"{model.ApplicationId} marked as Awaiting Payment successfully" });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AwaitingPayment([FromBody] dynamic model)
        {
            var application = await _ctx.Applications.FindAsync((string)model.ApplicationId);
            var order = await _ctx.CardOrders.FindAsync((string)model.OrderId);

            application.Status = ApplicationStatus.AwaitingPayment;
            await _ctx.SaveChangesAsync();

            var user = await _ctx.Users.FindAsync(this.GetUserId());
            await _emailSender.ApplicationAwaitingPaymentAsync(user, application);

            // create oder (setup callback) here now -- 
            if (_env.IsProduction())
            {
                await _cardOrder.CreateCardOrder(order.CardOrderId, order.CryptoAddress);
            }

            return Ok(new { Status = "success", Message = $"{model.ApplicationId} marked as Awaiting Payment successfully" });
        }

        [HttpGet("[action]/{appId}/{docType}")]
        public async Task<Document> GetDocumentInfo(string appId, string docType)
        {
            var documentType = docType.ParseEnum<DocumentTypes>();
            var application = await _ctx.Applications.FindAsync(appId);

            var document = await _ctx.Documents.Where(doc => doc.ApplicationId == appId && doc.DocumentType == documentType)
                .Select(d => new Document { ApplicationId = d.ApplicationId, DocumentId = d.DocumentId, DocumentType = d.DocumentType, FileType = d.FileType }).FirstOrDefaultAsync();

            if (document == null)
            {
                // create new --
                document = new Document
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    DocumentType = documentType,
                    ApplicationId = appId,
                    Order = 0,
                    FileType = "none",
                    Image = new byte[1],
                };

                await _ctx.Documents.AddAsync(document);
                await _ctx.SaveChangesAsync();
            }

            return document;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateDocument(UploadFileViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var document = await _ctx.Documents.FindAsync(model.DocumentId);

            using (var stream = new MemoryStream())
            {
                await model.Image.CopyToAsync(stream);

                // will store and db then will convert to base64
                byte[] image = stream.ToArray();

                document.DocumentType = Enum.Parse<DocumentTypes>(model.DocumentType);
                document.FileType = model.Image.ContentType;
                document.Image = image;

                await _ctx.SaveChangesAsync();
            }

            document.Image = null;

            return Ok(new
            {
                Status = "success",
                Message = $"Document Id {document.DocumentId} updated successfully!",
                docInfo = document
            });
        }

        [AllowAnonymous]
        [HttpGet("[action]/{documentId}")]
        public FileStreamResult GetFile(string documentId)
        {
            var doc = _ctx.Documents.Find(documentId);
            var app = _ctx.Applications.Find(doc.ApplicationId);

            var stream = new MemoryStream(doc.Image);
            return new FileStreamResult(stream, doc.FileType)
            {
                FileDownloadName = $"{app.ApplicationNumber}-{doc.DocumentType}.{doc.FileType.Replace("image/", "").Replace("application/", "")}"
            };
        }

        [HttpGet("[action]/{documentId}")]
        public List<string> GetFileImage(string documentId)
        {
            var doc = _ctx.Documents.Find(documentId);

            List<string> images = new List<string>();

            images.Add(doc.ImageSrcBase64);

            return images;
        }

        [HttpGet("[action]/{appId}")]
        public async Task<Address> GetAddress(string appId)
        {
            var application = await _ctx.Applications.FindAsync(appId);
            var user = await _ctx.Users.FindAsync(application.UserId);

            Address address;
            if (string.IsNullOrWhiteSpace(application.AddressId))
            {
                // create new --
                address = new Address
                {
                    AddressId = Guid.NewGuid().ToString(),
                    UserId = application.UserId,
                    Name = user.FirstName + " " + user.LastName,
                    AddressLine1 = "",
                    AddressLine2 = "",
                    City = "",
                    PostalCode = "",
                    Country = "",
                    Phone = user.PhoneNumber,
                };

                application.AddressId = address.AddressId;

                await _ctx.Addresses.AddAsync(address);
                await _ctx.SaveChangesAsync();
            }
            else
            {
                address = await _ctx.Addresses.FindAsync(application.AddressId);
            }

            return address;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateAddress([FromBody] Address model)
        {

            if (string.IsNullOrWhiteSpace(model.Province)) model.Province = model.City;

            var address = await _ctx.Addresses.FindAsync(model.AddressId);
            _ctx.Entry(address).CurrentValues.SetValues(model);

            await _ctx.SaveChangesAsync();

            return Ok(new { Status = "success", Message = $"Address {model.AddressId} updated successfully!" });
        }

        [HttpGet("[action]/{appId}")]
        public async Task<ShippingViewModel> GetShippingAddressAndShippingOptions(string appId)
        {
            var application = await _ctx.Applications.FindAsync(appId);
            var user = await _ctx.Users.FindAsync(application.UserId);

            Address address;
            if (string.IsNullOrWhiteSpace(application.ShippingAddressId))
            {
                // create new --
                address = new Address
                {
                    AddressId = Guid.NewGuid().ToString(),
                    UserId = application.UserId,
                    Name = user.FirstName + " " + user.LastName,
                    AddressLine1 = "",
                    AddressLine2 = "",
                    City = "",
                    PostalCode = "",
                    Country = "",
                    Phone = user.PhoneNumber,
                };

                application.ShippingAddressId = address.AddressId;

                await _ctx.Addresses.AddAsync(address);
                await _ctx.SaveChangesAsync();
            }
            else
            {
                address = await _ctx.Addresses.FindAsync(application.ShippingAddressId);
            }

            var selectedShippingOption = -1;
            List<ShippingOption> shippingOptions = null;
            if (!string.IsNullOrWhiteSpace(application.ShippingOptions))
            {
                shippingOptions = JsonConvert.DeserializeObject<List<ShippingOption>>(application.ShippingOptions);
                selectedShippingOption = application.SelectedShippingOption;
            }

            return new ShippingViewModel
            {
                ApplicationId = appId,
                Address = address,
                ShippingDetail = new ShippingDetail()
                {
                    Description = shippingOptions[0]?.Description, 
                    ShippingOptions = shippingOptions
                },
                SelectedShippingOption = selectedShippingOption
            };
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateShippingAddressAndShippingOptions([FromBody] ShippingViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var app = await _ctx.Applications.FindAsync(model.ApplicationId);

            if (model.SelectedShippingOption != -1)
            {
                app.ShippingCost = model.ShippingDetail.ShippingOptions[model.SelectedShippingOption].Price; // potential fraud, price can be altered on the client side, but we are reviewing each app -- 
                app.ShippingCarrier = model.ShippingDetail.ShippingOptions[model.SelectedShippingOption].Carrier;
                app.SelectedShippingOption = model.SelectedShippingOption;
                app.ShippingOptions = JsonConvert.SerializeObject(model.ShippingDetail.ShippingOptions);
            }

            if (string.IsNullOrWhiteSpace(model.Address.Province)) model.Address.Province = model.Address.City;
            var address = await _ctx.Addresses.FindAsync(model.Address.AddressId);
            _ctx.Entry(address).CurrentValues.SetValues(model.Address);

            await _ctx.SaveChangesAsync();

            return Ok(new { Message = $"Address {model.ApplicationId} updated successfully!" });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetShippingOptions([FromBody] dynamic model)
        {
            var currencyCode = ((string)model.currencyCode).ParseEnum<CurrencyCodes>();
            var countryCode = (string)model.countryCode;
            var postalCode = (string)model.postalCode;

            var options = await _shippingCalc.GetShippingOptionsAsync(currencyCode, countryCode, postalCode);

            return Ok(new { options });
        }


        [HttpGet("[action]/{appId}")]
        public async Task<IActionResult> GetOrderCost(string appId)
        {
           var application = await _ctx.Applications.FindAsync(appId);

            var total = await CalcOrderCost(application);

            return Ok(new { application.CardCost, application.ShippingCost, total = Math.Round(total, 2) });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetOrderDetails([FromBody] dynamic model)
        {
            var app = await _ctx.Applications.FindAsync((string)model.appId);

            // 0. send to API
            var cryptoCurrencyCode = ((string)model.cryptoCurrencyCode).ParseEnum<CryptoCurrencyCodes>();
            var currencyCode = app.CurrencyCode;
            double amountToPay = await CalcOrderCost(app);

            var orderId = Guid.NewGuid().ToString();

            if (app.ApplicationNumber == "1000001001-0001") // Andrei's test application -- 
            {
                amountToPay = 1;
            }

            // 1. call services to get an address and order details 
            //var cardOrder = await _cardOrder.GetForwardingAddress(amountToPay, currencyCode.ToString(), cryptoCurrencyCode.ToString());

            CardOrderDetails cardOrder = null;
            if (!_env.IsProduction())
            {
                Thread.Sleep(1000);
                cardOrder = CreateTestOrder();
            }
            else
            {
                cardOrder = await _cardOrder.GetForwardingAddress(amountToPay, currencyCode.ToString(), cryptoCurrencyCode.ToString());
            }

            // 2. save order in the database --           
            _ctx.CardOrders.Add(new CardOrder
            {
                ApplicationId = app.ApplicationId,
                CardOrderId = orderId,
                CryptoCurrencyCode = cryptoCurrencyCode.ToString().ParseEnum<CurrencyCodes>(),
                PurchaseCurrencyCode = currencyCode,
                CryptoAmount = cardOrder.Amount,
                PurchaseAmount = amountToPay,
                Comments = cardOrder.Comment,
                CryptoAddress = cardOrder.CryptoAddress,
                CryptoAddressQRCode = cardOrder.QrCodeSrc,
                Status = CardOrderStatuses.Created
            });

            await _ctx.SaveChangesAsync();

            //Thread.Sleep(5000);

            //await _cardOrder.CreateCardOrder(orderId, cardOrder.CryptoAddress);

            return Ok(new
            {
                CryptoCurrencyCode = cryptoCurrencyCode.ToString(),
                CurrencyCode = currencyCode.ToString(),
                orderId,
                cardOrder.Amount,
                cardOrder.CryptoAddress,
                cardOrder.QrCodeSrc,
                cardOrder.Comment
            });
        }

        async Task<string> CreateNewApplication(string userId, CurrencyCodes currencyCode, AccountProviders provider)
        {
            var user = await _ctx.Users.Include(u => u.Applications).FirstAsync(u => u.Id == userId);

            var app = new Application
            {
                UserId = userId,
                ApplicationId = Guid.NewGuid().ToString(),
                ApplicationNumber = GenerateApplicationNumber(user),
                Status = ApplicationStatus.Started,
                AccountType = AccountTypes.UnionPayDebit, // so far we have only one product, selection most likely to come from UI --
                CurrencyCode = currencyCode,
                ProviderName = provider,

                Title = Titles.Unknown,
                Gender = Genders.Unknown,
                MaritalStatus = MaritalStatuses.Unknown,

                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                DateOfBirth = "",

                CardCost = CalcCardCost(currencyCode),
            };

            user.Applications.Add(app);
            await _ctx.SaveChangesAsync();

            // generate references --
            long num = Convert.ToInt64(app.ApplicationNumber.Replace("-", ""));
            app.Reference = _refGen.GenAppRef(num);

            await _ctx.SaveChangesAsync();

            return app.ApplicationId;
        }

        CardOrderDetails CreateTestOrder()
        {
            // LTC for EUR
            return new CardOrderDetails
            {
                Amount = 0.7481035,
                CryptoAddress = "**** DEV 3K3c8cUHFrYqWkfnNddp24JcwC9i9ad5k5 DEV ***",
                QrCodeSrc = "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",
            };
        }

        double CalcCardCost(CurrencyCodes currencyCode)
        {
            return (currencyCode == CurrencyCodes.USD) ? 30 : 25;
        }

        async Task<double> CalcOrderCost(Application app)
        {
            if (app.ShippingCost == 0)
            {
                app.ShippingCost = (app.CurrencyCode == CurrencyCodes.USD) ? 50 : 45;
                await _ctx.SaveChangesAsync();
            }
            if (app.CardCost == 0)
            {
                app.CardCost = CalcCardCost(app.CurrencyCode);
                await _ctx.SaveChangesAsync();
            }
            return app.CardCost + app.ShippingCost;
        }

        string GenerateApplicationNumber(ApplicationUser user)
        {
            if (user.Applications == null || user.Applications?.Count == 0)
            {
                return user.UserNumber.ToString() + "-0001";
            }
            else
            {
                var apps = user.Applications.OrderByDescending(a => a.ApplicationNumber).ToList();
                var nextNumber = Convert.ToUInt64(apps[0].ApplicationNumber.Replace("-", "")) + 1;
                var num = nextNumber.ToString().Insert(10, "-");
                return num;
            }
        }
    }
}
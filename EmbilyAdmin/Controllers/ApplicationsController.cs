using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EmbilyAdmin.ViewModels;
using AspNet.Security.OAuth.Validation;
using Embily.Gateways;
using Embily.Gateways.CCSPrepay;
using Embily.Gateways.CCSPrepay.Models;
using Embily.Gateways.CoinPayInTh;
using Embily.Gateways.CoinPayInTh.Models;
using Embily.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Embily.Services;
using AutoMapper;
using System.Threading;

namespace EmbilyAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class ApplicationsController : Controller
    {
        public class ApplicationMapppingProfile : Profile
        {
            public ApplicationMapppingProfile()
            {
                CreateMap<ApplicationViewModel, Application>()
                    .ForMember(dest => dest.Documents, opts => opts.Ignore())
                    .ForMember(dest => dest.CardNumber, opts => opts.MapFrom(src => string.IsNullOrWhiteSpace(src.CardNumber) ? "" : src.CardNumber.Replace(" ", "").Replace("-", "")))
                    ;

                CreateMap<DocumentViewModel, Document>();

                CreateMap<Application, Account>()
                    .ForMember(dest => dest.AccountId, opts => opts.MapFrom(src => Guid.NewGuid()))
                    .ForMember(dest => dest.AccountNumber, opts => opts.MapFrom(src => src.ApplicationNumber))
                    .ForMember(dest => dest.AccountName, opts => opts.MapFrom(src => $"{src.User.Program.Title} Card ({src.CurrencyCode})"))
                    ;
            }
        }

        readonly EmbilyDbContext _ctx;
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly ILogger<ApplicationsController> _logger;
        readonly IRefGen _refGen;
        readonly IEmailQueueSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ICardHolder _cardHolder;

        public ApplicationsController(
            EmbilyDbContext ctx,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailQueueSender emailSender,
            ILogger<ApplicationsController> logger,
            IRefGen refGen,
            IMapper mapper,
            ICardHolder cardHolder
        )
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _refGen = refGen;
            _mapper = mapper;
            _cardHolder = cardHolder;
        }

        [HttpGet("[action]")]
        public IEnumerable<Application> GetAll()
        {
            return _ctx.Applications
                .Include(u => u.User)
                .OrderByDescending(app => app.DateCreated);
        }

        [HttpGet("[action]")]
        public IEnumerable<Application> GetNew()
        {
            return _ctx.Applications
                .Include(u => u.User)
                .Where(a => a.Status == ApplicationStatus.Submitted || a.Status == ApplicationStatus.Paid)
                .OrderByDescending(app => app.DateCreated);
        }

        [HttpGet("[action]/{applicationId}")]
        public async Task<ApplicationViewModel> Get(string applicationId)
        {
            var application = await _ctx.Applications.FindAsync(applicationId);

            var documents = _ctx.Documents.Where(d => d.ApplicationId == applicationId).OrderBy(o => o.Order)
                .Select(d => new Document
                {
                    ApplicationId = d.ApplicationId,
                    DocumentId = d.DocumentId,
                    Order = d.Order,
                    DocumentType = d.DocumentType,
                    FileType = d.FileType
                }).ToList();

            var docs = _mapper.Map<List<DocumentViewModel>>(documents);
            var app = _mapper.Map<ApplicationViewModel>(application);
            app.Documents = docs;

            return app;
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete([FromBody] dynamic model)
        {
            var appId = (string)model.applicationId;

            var account = await _ctx.Accounts.FindAsync(appId);
            if (account != null)
            {
                return BadRequest(new { status = "error", message = "Unable to remove. Account exists for this application. " });
            }

            var app = await _ctx.Applications
                 .Include(a => a.Documents)
                 .Include(a => a.ShippingAddress)
                 .Include(a => a.Address)
                 .FirstOrDefaultAsync(a => a.ApplicationId == appId);

            //var address = await _ctx.Addresses.FindAsync(a);
            try
            {
                _ctx.Remove(app);
                if (app.Address != null) _ctx.Remove(app.Address);
                if (app.ShippingAddress != null) _ctx.Remove(app.ShippingAddress);

                await _ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }

            return Ok(new { status = "success", message = $"Application {app.ApplicationNumber} deleted successfully!" });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody] ApplicationViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { status = "error", message = "invalid ViewModel" });

            var appdetails = await _ctx.Applications.FindAsync(model.ApplicationId);
            if (appdetails == null) return BadRequest(new { status = "error", message = "Application not found" });

            var appUpdated = _mapper.Map<ApplicationViewModel, Application>(model);

            _ctx.Entry(appdetails).CurrentValues.SetValues(appUpdated);
            await _ctx.SaveChangesAsync();

            //Thread.Sleep(5000);

            return Ok(new { status = "success", message = $"Application {appdetails.ApplicationNumber} updated successfully!", appStatus = appdetails.Status.ToString() });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateStatus([FromBody] ApplicationStatusViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { status = "error", message = "invalid ViewModel" });

            var appdetails = await _ctx.Applications.FindAsync(model.ApplicationId);
            if (appdetails == null) return BadRequest(new { status = "error", message = "Application not found" });

            appdetails.Status = model.Status.ParseEnum<ApplicationStatus>();
            appdetails.Comments = JsonConvert.SerializeObject(model.StatusDesc);
            await _ctx.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(appdetails.UserId);

            // notification email handling --
            switch (appdetails.Status)
            {
                case ApplicationStatus.Paid:
                    await _emailSender.ApplicationPaidAsync(user, appdetails);
                    break;
                case ApplicationStatus.Rejected:
                    await _emailSender.ApplicationRejectedAsync(user, appdetails, model.StatusDesc);
                    break;
                case ApplicationStatus.Shipped:
                    if (appdetails.ShippingCarrier == "Pickup")
                    {
                        await _emailSender.ApplicationPickedUpAsync(user, appdetails);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(appdetails.ShippingTrackingNum) || string.IsNullOrWhiteSpace(appdetails.ShippingCarrier))
                            return BadRequest(new { status = "error", message = "Carrier and/or shipping number has not been saved to the database! Please, correct and try again!" });

                        await ProcessShipping(user, appdetails);
                    }
                    break;
            }

            return Ok(new
            {
                status = "success",
                message = $"Application {appdetails.ApplicationNumber} status successfully updated!",
                appStatus = appdetails.Status.ToString(),
                appStatusComments = appdetails.Comments,
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterAndSendForKYCApproval([FromBody] ApplicationIdViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { status = "error", message = "invalid ViewModel" });

            var appdetails = await _ctx.Applications
                .Include(a => a.Address)
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.ApplicationId == model.ApplicationId);

            if (appdetails == null) return BadRequest(new { status = "error", message = "Application not found" });

            var user = await _ctx.Users.FindAsync(appdetails.UserId);

            string cardHolderID = "";
            try
            {
                cardHolderID = await _cardHolder.RegisterWithDocs(appdetails); 
                await _userManager.AddToRoleAsync(user, Roles.Client_Registered.ToString());

                appdetails.ProviderUserId = cardHolderID;
                appdetails.Status = ApplicationStatus.Processing;
                await _ctx.SaveChangesAsync();

                await _emailSender.ApplicationProcessingAsync(user, appdetails);
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "error", message = e.Message });
            }

            return Ok(new
            {
                Status = "success",
                Message = $"Application {appdetails.ApplicationNumber} submitted for approval successfully!",
                AppStatus = appdetails.Status.ToString(),
                ClientId = cardHolderID
            });
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AssignCard([FromBody] ApplicationViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { status = "error", message = "invalid ViewModel" });

            var appdetails = await _ctx.Applications.FindAsync(model.ApplicationId);
            if (appdetails == null) return BadRequest(new { status = "error", message = "Application not found" });

            var appUpdated = _mapper.Map<ApplicationViewModel, Application>(model);

            _ctx.Entry(appdetails).CurrentValues.SetValues(appUpdated);
            await _ctx.SaveChangesAsync();

            if(string.IsNullOrWhiteSpace(appdetails.ProviderUserId) || string.IsNullOrWhiteSpace(appdetails.ProviderAccountNumber) && string.IsNullOrWhiteSpace(appdetails.CardNumber))
                return BadRequest(new { status = "error", message = "User Reference or Card Reference or Card Number are not specified." });

            try
            {
                await _cardHolder.AssignCard(appdetails.ProviderUserId, appdetails.ProviderAccountNumber);
            }
            catch(Exception ex)
            {
                string msg = (ex.InnerException != null)? ex.Message : ex.InnerException.Message;
                return BadRequest(new { status = "error", message = $"Card [{appdetails.CardNumber}], Card Reference ID [{appdetails.ProviderAccountNumber}] error: {msg}." } );
            }
            return Ok(new { status = "success", message = $"Card {appdetails.CardNumber} assigned successfully!" });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateAccountKYC([FromBody] ApplicationIdViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { status = "error", message = "invalid ViewModel" });

            var appdetails = await _ctx.Applications.FindAsync(model.ApplicationId);
            if (appdetails == null) return BadRequest(new { status = "error", message = "Application not found" });

            if (appdetails.ProviderName == AccountProviders.Virtual)
            {
                GenVirtualReferences(appdetails);
            }

            if (string.IsNullOrWhiteSpace(appdetails.ProviderAccountNumber) 
                || string.IsNullOrWhiteSpace(appdetails.ProviderUserId)
                || string.IsNullOrWhiteSpace(appdetails.CardNumber))
                return BadRequest(new { status = "error", message = "Unable to create an account. Missing ProviderAccountNumber and/or ProviderUserId " });

            var user = await _ctx.Users.FindAsync(appdetails.UserId);
            var progrma = await _ctx.Programs.FindAsync(user.ProgramId);

            try
            {
                await CreateAccountDB(appdetails);

                appdetails.Status = ApplicationStatus.Approved;
                await _ctx.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, "Client_KYC");

                await _emailSender.ApplicationAppovedKYCAsync(user, appdetails);
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "error", message = e.Message });
            }

            return Ok(new { status = "success", message = $"Application {appdetails.ApplicationNumber} account created successfully!", appStatus = appdetails.Status.ToString() });
        }

        [HttpGet("[action]/{applicationId}")]
        public async Task<List<CardOrder>> GetCardOrderList(string applicationId)
        {
            List<CardOrder> cardorderlist = _ctx.CardOrders.Where(co => co.ApplicationId == applicationId).OrderByDescending(d => d.DateCreated).ToList();

            return await Task.Run(() => cardorderlist);
        }

        [AllowAnonymous]
        [HttpGet("[action]/{documentId}")]
        public async Task<IActionResult> GetFilePdf(string documentId)
        {
            var doc = await _ctx.Documents.FindAsync(documentId);
            return File(doc.Image, doc.FileType); // FileStreamResult
        }

        [HttpGet("[action]/{documentId}")]
        public List<string> GetFileImage(string documentId)
        {
            var doc = _ctx.Documents.Find(documentId);

            List<string> images = new List<string>();

            images.Add(doc.ImageSrcBase64);

            return images;
        }

        private void GenVirtualReferences(Application appdetails)
        {
            appdetails.ProviderUserId = appdetails.Reference;
            var r = new Random();
            var d = Math.Round((r.NextDouble() * 10000000000));
            var cardNumber = $"523075{d}";

            appdetails.ProviderAccountNumber = cardNumber;
        }

        async Task CreateAccountDB(Application appdetails)
        {
            var acct = _mapper.Map<Application, Account>(appdetails);

            _ctx.Accounts.Add(acct);
            await _ctx.SaveChangesAsync();
        }

        private async Task ProcessShipping(ApplicationUser user, Application appdetails)
        {
            string trackingUrl = "";

            switch (appdetails.ShippingCarrier)
            {
                case "DHL":
                    trackingUrl = $"http://www.dhl.com/en/express/tracking.html?AWB={appdetails.ShippingTrackingNum}&brand=DHL";
                    break;
                case "Kerry Express":
                    trackingUrl = $"https://th.kerryexpress.com/en/track/?track={appdetails.ShippingTrackingNum}";
                    break;
            }

            await _emailSender.ApplicationShippedAsync(user, appdetails, trackingUrl);
        }
    }
}
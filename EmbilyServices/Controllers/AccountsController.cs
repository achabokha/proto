using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;


using Embily.Gateways;
using Embily.Gateways.KoKard;
using Embily.Gateways.KoKard.Models;
using Embily.Messages;
using Embily.Models;
using Embily.Services;
using EmbilyServices.Hubs;
using EmbilyServices.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using QRCoder;
//using ZXing.QrCode;

namespace EmbilyServices.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly EmbilyDbContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICryptoAddress _cryptoAddress;
        private readonly ICard _card;
        private readonly ILogger _logger;
        private readonly IRefGen _refGen;
        private readonly IEmailQueueSender _emailSender;
        private readonly IHubContext<BlockchainHub> _hubContext;

        public AccountsController(
            IHostingEnvironment env,
            ILogger<AccountsController> logger,
            EmbilyDbContext ctx,
            UserManager<ApplicationUser> userManager,
            ICryptoAddress cryptoAddress,
            ICard card,
            IRefGen refGen,
            IEmailQueueSender emailSender,
            IHubContext<BlockchainHub> hubContext
        )
        {
            _env = env;
            _logger = logger;
            _ctx = ctx;
            _userManager = userManager;
            _cryptoAddress = cryptoAddress;
            _card = card;
            _refGen = refGen;
            _emailSender = emailSender;
            _hubContext = hubContext;
        }

        private string GetUserId()
        {
            return User.GetClaim(OpenIdConnectConstants.Claims.Subject);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDashboardInfo()
        {
            string userId = this.GetUserId();

            var apps = _ctx.Applications.Where(a => a.UserId == userId)
                .Select(app => new Application
                {
                    ApplicationId = app.ApplicationId,
                    ApplicationNumber = app.ApplicationNumber,
                    Reference = app.Reference,
                    CurrencyCode = app.CurrencyCode,
                    Status = app.Status,
                    DateCreated = app.DateCreated,
                    Comments = app.Comments
                })
                .OrderBy(o => o.DateCreated);

            var accounts = _ctx.Accounts.Where(a => a.UserId == userId
                                                && a.AccountType != AccountTypes.Affiliate
                                                && a.AccountStatus != AccountStatuses.Suspended
                                                && a.AccountStatus != AccountStatuses.Closed);

            //await accounts.ForEachAsync(a => a.Balance = null);

            var EditApps = new List<Application>();
            var SubmittedApps = new List<Application>();
            var allApps = new List<Application>();

            if (apps != null)
            {
                foreach (var app in apps)
                {
                    Application appstatus = (app.Status == ApplicationStatus.Approved || app.Status == ApplicationStatus.Shipped) ? null : app;

                    if (appstatus != null)
                    {
                        if (appstatus.Status == ApplicationStatus.Submitted)
                        {
                            SubmittedApps.Add(appstatus);
                        }
                        else
                        {
                            EditApps.Add(appstatus);
                        }
                    }
                }
                allApps.AddRange(SubmittedApps);
                allApps.AddRange(EditApps);
            }

            var user = await _userManager.FindByIdAsync(userId);
            var isAffiliate = await _userManager.IsInRoleAsync(user, Roles.Affiliate.ToString());

            var affiliateAccounts = !isAffiliate ? null : _ctx.Accounts.Where(a => a.UserId == userId && a.AccountType == AccountTypes.Affiliate);

            var stage = accounts.Count() == 0 && apps.Count() == 0 && !isAffiliate ? "fresh" : "cool";

            return Ok(new
            {
                Stage = stage,
                Accounts = accounts,
                Applications = allApps,
                isAffiliate,
                affiliateAccounts
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCardAccounts()
        {
            string userId = this.GetUserId();

            var accounts = await _ctx.Accounts.Where(a => a.UserId == userId && a.AccountType != AccountTypes.Affiliate).ToListAsync();

            return Ok(new { Accounts = accounts });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> IsRoleAffiliate()
        {
            string userId = this.GetUserId();

            var user = await _userManager.FindByIdAsync(userId);
            var isAffiliate = await _userManager.IsInRoleAsync(user, Roles.Affiliate.ToString());

            return Ok(new { isAffiliate });
        }

        [HttpGet("[action]/{accountId}")]
        public async Task<IActionResult> GetBalance(string accountId)
        {
            var account = await _ctx.Accounts.FindAsync(accountId);
            if (account == null) return BadRequest(new { Message = $"Account {accountId} not found" });

            double balance = 0;

            if (account.AccountType == AccountTypes.Affiliate)
            {
                balance = this.GetAffiliateAccountBalance(account);
            }
            else
            {
                try
                {
                    balance = await _card.GetCardBalanceAsync(account.ProviderUserId, account.ProviderAccountNumber);
                }
                catch(Exception e)
                {
                    return Ok(new { balance = 0, exception = e.Message, internalException = e.InnerException?.Message });
                } 
            }
            //balance = 154.4456;

            return Ok(new { balance = Math.Round(balance, 2) });
        }

        private double GetAffiliateAccountBalance(Account account)
        {
            var credit = _ctx.Transactions.Where(t => t.AccountId == account.AccountId && t.TxnType == TxnTypes.CREDIT).Sum(i => i.DestinationAmount);
            var debit = _ctx.Transactions.Where(t => t.AccountId == account.AccountId && t.TxnType == TxnTypes.DEBIT).Sum(i => i.DestinationAmount);
            return Math.Round(credit - debit, 2); ;
        }

        [HttpGet("[action]/{currencyCode}")]
        public List<Account> GetAccountsByCurrency(string currencyCode)
        {
            string userId = this.GetUserId();

            var cCode = currencyCode.ParseEnum<CurrencyCodes>();
            var accounts = _ctx.Accounts.Where(a => a.UserId == userId
                && a.CurrencyCode == cCode
                && a.AccountType != AccountTypes.Affiliate)
                .OrderBy(o => o.DateCreated);

            return accounts.ToList();
        }


        [HttpGet("[action]")]
        public List<Account> GetAccounts()
        {
            string userId = this.GetUserId();
            var accounts = _ctx.Accounts.Where(a => a.UserId == userId).ToList();

            _logger.LogInformation($">>> User [ID: {userId}] has {accounts.Count} account(s).");
            return accounts;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAffiliateTransactions()
        {
            string userId = this.GetUserId();

            var accounts = await _ctx.Accounts.Include(a => a.Transactions)
                .Where(a => a.UserId == userId && a.AccountType == AccountTypes.Affiliate && a.AccountStatus == AccountStatuses.Active)
                .ToListAsync();

            var transactions = new List<Transaction>();

            foreach (var account in accounts)
            {
                transactions.AddRange(account.Transactions);
            }

            return Json(transactions.OrderByDescending(txn => txn.DateCreated));
        }

        [HttpGet("[action]")]
        public JsonResult GetLoadTransactions()
        {
            string userId = this.GetUserId();

            var txns = _ctx.Transactions.Include(a => a.Account).Where(
                t => t.Account.UserId == userId
                && t.Account.AccountType != AccountTypes.Affiliate
                && t.TxnCode != TxnCodes.VIRTUALCARD_LOAD).OrderByDescending(o => o.DateCreated);

            return Json(txns);
        }

        [HttpGet("[action]")]
        public async Task<List<TransactionInfo>> GetAllTransactions()
        {
            string userId = this.GetUserId();
            var accounts = _ctx.Accounts.Where(a => a.UserId == userId
                                                && a.AccountType != AccountTypes.Affiliate
                                                && a.AccountStatus == AccountStatuses.Active);

            var transactions = new List<TransactionInfo>();
            foreach (var account in accounts)
            {
                var txns = await _card.GetTransactionsAsync(account.ProviderUserId, account.ProviderAccountNumber);
                transactions.AddRange(txns);
            }

            return transactions;
        }

        [HttpGet("[action]/{currencyCode}/{accountId}")]
        public async Task<IActionResult> GetNewAddress(string currencyCode, string accountId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(this.GetUserId());
            var account = await _ctx.Accounts.FindAsync(accountId);
            if (account == null) return BadRequest(new { Status = "error", ErrorMessage = "account not found" });

            var code = currencyCode.ParseEnum<CurrencyCodes>();

            var cryptoAddress = _ctx.CryptoAddresses.Where(adr => adr.AccountId == accountId && adr.CurrencyCode == code).OrderByDescending(adr1 => adr1.DateCreated);

            string newAddress;
            if (cryptoAddress.Count() == 0)
            {
                // get a new address, store in DB then
                newAddress = await _cryptoAddress.GetNewAddressAsync(code, account.AccountNumber);
                var cryptoAdr = new CryptoAddress
                {
                    CryptoAddressId = Guid.NewGuid().ToString(),
                    Address = newAddress,
                    CurrencyCode = code,
                    AccountId = accountId,
                };
                _ctx.CryptoAddresses.Add(cryptoAdr);
                await _ctx.SaveChangesAsync();
            }
            else
            {
                newAddress = cryptoAddress.First().Address;
            }

            var currency = GetCurrencyName(code);
            var imgSrc = CreateBarCode(string.Format("{0}:{1}", currency, newAddress));

            return Ok(new
            {
                Status = "success",
                IsKYC = await _userManager.IsInRoleAsync(user, Roles.Client_KYC.ToString()),
                AccountCurrencyCode = account.CurrencyCode.ToString(),
                address = newAddress,
                qrCodeImgSrc = imgSrc,
            });
        }

        private static string GetCurrencyName(Embily.Models.CurrencyCodes code)
        {
            switch (code)
            {
                case Embily.Models.CurrencyCodes.BTC:
                    return "bitcoin";
                case Embily.Models.CurrencyCodes.LTC:
                    return "litecoin";
                case Embily.Models.CurrencyCodes.ETH:
                    return "ethereum";
                default:
                    throw new ApplicationException($"unsupported crypto currency code {code}");
            }
        }

        private string CreateBarCode(string address)
        {
            // https://github.com/codebude/QRCoder/wiki/Advanced-usage---Payload-generators#32-bitcoin-payment-address

            var generator = new PayloadGenerator.BitcoinAddress(address, null);
            string payload = generator.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
            return qrCodeImageAsBase64;
        }

        [HttpGet("[action]/{accountId}")]
        public async Task<IActionResult> SetLostCard(string accountId)
        {
            var account = await _ctx.Accounts.FindAsync(accountId);
            if (account == null) return BadRequest(new { Message = $"Account {accountId} not found" });

            var user = await _ctx.Users.Include(u => u.Applications).FirstAsync(u => u.Id == account.UserId);

            account.AccountStatus = AccountStatuses.Suspended;

            var oldApplication = await _ctx.Applications.Where(oapp => oapp.ApplicationNumber == account.AccountNumber).FirstOrDefaultAsync();

            Mapper.Initialize(cfg => cfg.CreateMap<Application, Application>());
            var newApplication = Mapper.Map<Application, Application>(oldApplication);

            newApplication.ApplicationId = Guid.NewGuid().ToString();
            newApplication.Status = ApplicationStatus.Started;
            newApplication.ApplicationNumber = GenerateApplicationNumber(user);

            long num = Convert.ToInt64(newApplication.ApplicationNumber.Replace("-", ""));
            newApplication.Reference = _refGen.GenAppRef(num);

            _ctx.Applications.Add(newApplication);

            await _ctx.SaveChangesAsync();

            await _emailSender.LostCardNewApplicationAlertAsync(user, newApplication, account);

            await _emailSender.InitialisationLostCardAsync(user, newApplication, account, oldApplication);

            return Ok(new { account, newApplication });
        }

        private string GenerateApplicationNumber(ApplicationUser user)
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

        [HttpPost("[action]")]
        public async Task<IActionResult> SetCardPIN([FromBody] SetPINViewModel model)
        {
            var _api = new KoKardAPI();

            SetCardPINResponse response;
            try
            {
                response = await _api.SetCardPINAsync(model.CardReferenceID, model.NewPIN);
            }
            catch
            {
                return BadRequest(new { status = "error", message = "PIN update failed!" });
            }


            return Ok(new { status = "success", message = $"PIN updated successfully!", response });

        }
    }
}
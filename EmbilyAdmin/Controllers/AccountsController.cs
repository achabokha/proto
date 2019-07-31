using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Authorization;

using AspNet.Security.OAuth.Validation;

using Embily.Models;
using System.Threading;
using EmbilyAdmin.ViewModels;
using Embily.Gateways.CCSPrepay;
using Embily.Gateways;
using Microsoft.AspNetCore.Hosting;
using Embily.Services;

namespace EmbilyAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin,CustomerSupport")]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        readonly IHostingEnvironment _env;
        readonly EmbilyDbContext _ctx;
        readonly ICardLoad _cardLoad;
        readonly ICryptoAddress _cryptoAddress;
        readonly IEmailQueueSender _emailSender;
        readonly IRefGen _refGen;

        public AccountsController(
            IHostingEnvironment env,
            EmbilyDbContext ctx,
            ICardLoad card,
            ICryptoAddress cryptoAddress,
            IEmailQueueSender emailSender,
            IRefGen refGen
            )
        {
            _env = env;
            _ctx = ctx;
            _cardLoad = card;
            _cryptoAddress = cryptoAddress;
            _emailSender = emailSender;
            _refGen = refGen;
        }

        [HttpGet("[action]")]
        public IEnumerable<Account> GetAccounts()
        {
            return _ctx.Accounts
                .Include(a => a.User)
                .ToList().OrderByDescending(a => a.DateCreated);
        }

        [HttpGet("[action]/{accountId}")]
        public async Task<Account> Get(string accountId)
        {
            var account = await _ctx.Accounts
               .Include(a => a.User)
               .Include(a => a.CryptoAddreses)
               .Include(a => a.Transactions)
               .FirstOrDefaultAsync(a => a.AccountId == accountId);

            account.Transactions = account.Transactions.OrderByDescending(t => t.DateCreated).ToList();

            return account;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountViewModel model)
        {
            var account = await _ctx.Accounts.FirstOrDefaultAsync(a => a.AccountId == model.AccountId);
            if (account == null)
            {
                return BadRequest(new { status = "error", message = "Account not found." });
            }
            try
            {
                account.CurrencyCodeString = model.CurrencyCodeString;
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.Phone = model.Phone;
                account.Email = model.Email;

                await _ctx.SaveChangesAsync();
            }
            catch(Exception exc)
            {
                return BadRequest(new { status = "error", message = "Account was not saved." });
            }

            return Ok(new { status = "success", message = $"Account details updated successfully!"});
        }

        [HttpGet("[action]/{accountId}/{currencyCode}")]
        public async Task<IActionResult> GetNewCryptoAddress(string accountId, string currencyCode)
        {
            var account = await _ctx.Accounts.FindAsync(accountId);
            if (account == null) return BadRequest(new { Status = "error", ErrorMessage = "account not found" });

            var code = currencyCode.ParseEnum<CurrencyCodes>();

            // get a new address, store in DB then
            string newAddress = await _cryptoAddress.GetNewAddressAsync(code, account.AccountNumber);
            var cryptoAdr = new CryptoAddress
            {
                CryptoAddressId = Guid.NewGuid().ToString(),
                Address = newAddress,
                CurrencyCode = code,
                AccountId = accountId,
            };

            _ctx.CryptoAddresses.Add(cryptoAdr);
            await _ctx.SaveChangesAsync();

            return Ok(new
            {
                Status = "success",
                Address = newAddress,
                CurrencyCode = currencyCode,
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetTransactions([FromBody] PageTableViewModel page)
        {           
            var transactions = await _ctx.Transactions.Include(t => t.Account).OrderByDescending(t => t.DateCreated).ToListAsync();
            var pageTransactions = new List<Transaction>();
            page.TotalElements = transactions.Count;
            page.TotalPages = page.TotalElements / page.Size;
            var start = page.PageNumber * page.Size;
            var end = Math.Min((start + page.Size), page.TotalElements);
            for(int i = start; i < end; i++)
            {
                var transaction = transactions[i];
                pageTransactions.Add(transaction);
            }
            return Ok(new { pageTransactions, page });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Load([FromBody] AccountLoadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { Status = "error", Message = "Invalid ViewModel" });

            var account = await _ctx.Accounts.FindAsync(model.AccountId);
            if (account == null) return BadRequest(new { Status = "error", Message = "Account not found" });

            if (string.IsNullOrWhiteSpace(account.ProviderUserId)) return BadRequest(new { Status = "error", Message = "Missing ProviderAccountNumber" });

            try
            {
                await _cardLoad.LoadCardAsync(account.ProviderUserId, account.ProviderAccountNumber, model.Amount, null);

                // create transaction
                var txn = await CreateAndSaveTransactionDB(account, model.Amount, TxnStatus.Complete);

                // send notification email 
                var user = await _ctx.Users.FindAsync(account.UserId);
                await _emailSender.TopupLoadAsync(user, txn, account);
            }
            catch (Exception e)
            {
                return BadRequest(new { Status = "error", e.Message });
            }

            return Ok(new
            {
                Status = "success",
                Message = $"Account {account.AccountNumber} loaded with {account.CurrencyCode} {model.Amount} successfully!",
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateAffiliateAccount([FromBody] dynamic model)
        {
            string userId = (string)model.userId;
            var user = await _ctx.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.Id == userId);
            var currencyCode = ((string)model.currencyCode).ParseEnum<CurrencyCodes>();

            var account = new Account
            {
                AccountId = Guid.NewGuid().ToString(),
                AccountNumber = GenerateAffiliateAccountNumber(user),
                AccountType = AccountTypes.Affiliate,
                AccountName = $"{user.Program.Title} Affiliate ({currencyCode})",
                CurrencyCode = currencyCode,
                ProviderName = AccountProviders.Embily,
                ProviderAccountNumber = "",
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
            };

            _ctx.Accounts.Add(account);
            await _ctx.SaveChangesAsync();

            return Ok(new
            {
                Status = "success",
                Message = $"An affiliate account {account.AccountNumber} ({account.CurrencyCode}) created successfully!",
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SetAccountSatus([FromBody] dynamic model)
        {
            var accountId = (string)model.accountId;
            var accountStatus = (string)model.accountStatus;
            var account = await _ctx.Accounts.FindAsync(accountId);

            account.AccountStatusString = accountStatus;

            await _ctx.SaveChangesAsync();

            return Ok(new { account });
        }

        private string GenerateAffiliateAccountNumber(ApplicationUser user)
        {
            if (user.Accounts == null || user.Accounts?.Count == 0)
            {
                return user.UserNumber.ToString() + "-0001";
            }
            else
            {
                var accounts = user.Accounts.OrderByDescending(a => a.AccountNumber).ToList();
                var nextNumber = Convert.ToUInt64(accounts[0].AccountNumber.Replace("-", "")) + 1;
                var num = nextNumber.ToString().Insert(10, "-");
                return num;
            }
        }

        async Task<Transaction> CreateAndSaveTransactionDB(Account account, double amount, TxnStatus status)
        {
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                //TransactionNumber = // generated by the database via a sequence 
                CryptoProvider = CryptoProviders.None,
                CryptoAddress = "",
                TxnType = TxnTypes.DEBIT,
                TxnCode = TxnCodes.LOAD_DIRECT,
                OriginalCurrencyCode = CurrencyCodes.NONE,
                OriginalAmount = 0,
                DestinationCurrencyCode = account.CurrencyCode,
                DestinationAmount = amount,
                IsAmountKnown = true,
                Status = status,
                AccountId = account.AccountId,
            };

            await _ctx.Transactions.AddAsync(transaction);
            await _ctx.SaveChangesAsync();

            // generate references --
            long num = Convert.ToInt64(transaction.TransactionNumber);
            transaction.Reference = _refGen.GenTxnRef(num);
            transaction.TxnGroupRef = _refGen.GenTxnGroupRef(num);

            await _ctx.SaveChangesAsync();

            return transaction;
        }
    }
}

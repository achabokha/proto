using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;
using Embily.Gateways;
using Embily.Models;
using Embily.Services;
using EmbilyServices.ViewModels;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EmbilyServices.Controllers
{
    //public class AffiliateMapProfile : Profile
    //{
    //    public AffiliateMapProfile()
    //    {
    //        //CreateMap<Application, ApplicationInfoViewModel>();
    //        //CreateMap<ApplicationInfoViewModel, Application>();
    //    }
    //}

    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AffiliateController : BaseController
    {
        readonly EmbilyDbContext _ctx;
        readonly IHostingEnvironment _env;
        readonly IMapper _mapper;
        readonly IEmailQueueSender _emailSender;
        readonly ICardLoad _card;
        readonly UserManager<ApplicationUser> _userManager;
        readonly IRefGen _refGen;

        public AffiliateController(
            UserManager<ApplicationUser> userManager,
            EmbilyDbContext ctx,
            IHostingEnvironment env,
            IEmailQueueSender emailSender,
            IMapper mapper,
            ICardLoad card,
            IRefGen refGen
            )
        {
            _userManager = userManager;
            _ctx = ctx;
            _env = env;
            _mapper = mapper;
            _emailSender = emailSender;
            _card = card;
            _refGen = refGen;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InviteNewUser([FromBody] dynamic model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var email = ((string)model.email).Trim();
            var normalizedEmail = email.ToUpperInvariant();

            string userId = this.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            if (user.NormalizedEmail == normalizedEmail) return BadRequest(new { Message = "Inviting yourself??? hmm..." });

            var tryUser = await _userManager.FindByEmailAsync(email);
            if(tryUser != null) return BadRequest(new { Message = "Unable to invite this user."});

            var findAffiliate = _ctx.AffiliateEmails.Where(a => a.Email == email).FirstOrDefault();
            if (findAffiliate == null)
            {
                var newAffiliate = new AffiliateEmail()
                {
                    AffiliateEmailId = Guid.NewGuid().ToString(),
                    Email = email,
                    NormalizedEmail = normalizedEmail,
                    UserId = userId,
                };

                _ctx.AffiliateEmails.Add(newAffiliate);

                await _ctx.SaveChangesAsync();
            }
            else
            {
                if(findAffiliate.UserId == userId)
                {
                    return BadRequest(new { Message = $"You have already invited this person." });
                }
                else
                {
                    return BadRequest(new { Message = $"This person has been invited by someone else." });
                }
            }

            await _emailSender.AffiliateInInviteAsync(user, email);


            return Ok(new { Message = $"A new user has been added to your network, an invitation email has been sent." });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateToken([FromBody] dynamic model)
        {
            // TODO: using this lil libary for shorter, YouTube like hashes --
            /// https://github.com/ullmark/hashids.net

            if (!ModelState.IsValid) return BadRequest(ModelState);

            string userId = this.GetUserId();
            string affiliateTokenId = Guid.NewGuid().ToString();
            string description = (string)model.description;

            var hashids = new Hashids(affiliateTokenId, 12);
            var token = hashids.Encode(1);

            var newToken = new AffiliateToken()
            {
                AffiliateTokenId = affiliateTokenId,
                Token = token,
                NormalizedToken = token.ToUpperInvariant(),
                UserId = userId,
                Description = description,
                Counter = 0,
                IsActive = true
            };

            _ctx.AffiliateTokens.Add(newToken);

            await _ctx.SaveChangesAsync();

            var tokenList = _ctx.AffiliateTokens.Where(t => t.UserId == userId && t.IsActive && !t.IsSuspended).ToList();

            var customTokenList = await TokensCountStats(tokenList);

            return Ok(new { Status = "success", tokenList = customTokenList, Message = $"complete successfully" });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeactivateToken([FromBody] dynamic model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string userId = this.GetUserId();
            var tokenId = (string)model.tokenId;

            var token = await _ctx.AffiliateTokens.FindAsync(tokenId);
            token.IsActive = false;
            await _ctx.SaveChangesAsync();

            var tokenList = _ctx.AffiliateTokens.Where(t => t.UserId == userId && t.IsActive && !t.IsSuspended).ToList();

            var customTokenList = await TokensCountStats(tokenList);

            return Ok(new { Status = "success", tokenList = customTokenList, Message = $"complete successfully" });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetTokens()
       {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string userId = this.GetUserId();

            var tokenList = await _ctx.AffiliateTokens.Where(t => t.UserId == userId && t.IsActive && !t.IsSuspended).ToListAsync();

            var customTokenList = await TokensCountStats(tokenList);


            return Ok(new { Status = "success", tokenList = customTokenList, Message = $"complete successfully" });
        }

        private async Task<List<AffiliateTokenViewModels>> TokensCountStats(List<AffiliateToken> tokenList)
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<AffiliateToken, AffiliateTokenViewModels>());
            var customTokenList = Mapper.Map<List<AffiliateToken>, List<AffiliateTokenViewModels>>(tokenList);

            foreach (var token in customTokenList)
            {
                var users = _ctx.Users.Where(u => u.AffiliateTokenUsed == token.AffiliateTokenId).ToList();
                token.CountRegistered = users.Count;

                int countApproved = 0;
                int countTransacting = 0;

                foreach (var user in users)
                {
                    var appAproved = await _ctx.Applications.Where(app => app.UserId == user.Id).Where(s => s.Status == ApplicationStatus.Approved).FirstOrDefaultAsync();
                    if (appAproved != null)
                    {
                        countApproved++;
                    }
                    var accounts = await _ctx.Accounts.Where(acc => acc.UserId == user.Id).ToArrayAsync();
                    foreach (var account in accounts)
                    {
                        var transactions = await _ctx.Transactions.Where(t => t.AccountId == account.AccountId).ToListAsync();
                        if (transactions != null || transactions.Count == 0)
                        {
                            countTransacting++;
                            break;
                        }
                    }
                }

                token.CountApplied = countApproved;

                token.CountTransacting = countTransacting;
            }

            return customTokenList;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RedeemBalance([FromBody] dynamic model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Account sourceAccount = await _ctx.Accounts.FindAsync((string)model.sourceAccountId);
            Account destinationAccount = await _ctx.Accounts.FindAsync((string)model.destinationAccountId);

            if (sourceAccount == null) BadRequest(new { Message = $"Source account not found" });
            if (destinationAccount == null) BadRequest(new { Message = $"Destination account not found" });
            var userId = this.GetUserId();

            if (userId != sourceAccount.UserId && userId != destinationAccount.UserId)
            {
                // possible fraud - affiliate balance can be only deposited to user's card account --
                // TODO: send email to finding --

                BadRequest(new { Message = $"Unable to process your balance redemption. Please, contact customer support." });
            }

            if (destinationAccount.AccountType == AccountTypes.Affiliate) BadRequest(new { Message = $"Destination account is an Affiliate account" });
            if (sourceAccount.AccountType != AccountTypes.Affiliate) BadRequest(new { Message = $"Account is not an Affiliate account" });
            if (sourceAccount.CurrencyCode != destinationAccount.CurrencyCode) BadRequest(new { Message = $"Accounts currencies do not match" });

            if (string.IsNullOrWhiteSpace(destinationAccount.ProviderUserId)) return BadRequest(new { Message = "Missing destination account ProviderAccountNumber" });

            double amount = GetAffiliateAccountBalance(sourceAccount);

            double limitRedeem = 10;
            if (!(amount >= limitRedeem)) BadRequest(new { Message = $"Insufficient funds, minimum redemption amount {sourceAccount.CurrencyCode} {limitRedeem.ToString("NB")}" });

            try
            {
                // call load API
                if (_env.IsProduction())
                {
                    await _card.LoadCardAsync(destinationAccount.ProviderUserId, destinationAccount.ProviderAccountNumber, amount, null);
                }
                else
                {
                    Thread.Sleep(1000);
                }

                // move funds around create transactions
                await CreateAndSaveTransactionsDB(sourceAccount, destinationAccount, amount);

                // send notification email 
                var user = await _ctx.Users.FindAsync(this.GetUserId());
                await _emailSender.AffiliateReedemAsync(user, destinationAccount, amount);
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }

            return Ok(new { Message = $"Balance redemption completed successfully!" });
        }

        private double GetAffiliateAccountBalance(Account account)
        {
            var credit = _ctx.Transactions.Where(t => t.AccountId == account.AccountId && t.TxnType == TxnTypes.CREDIT).Sum(i => i.DestinationAmount);
            var debit = _ctx.Transactions.Where(t => t.AccountId == account.AccountId && t.TxnType == TxnTypes.DEBIT).Sum(i => i.DestinationAmount);
            return Math.Round(credit - debit, 2);
        }

        async Task CreateAndSaveTransactionsDB(Account sourceAccount, Account destinationAccount, double amount)
        {
            var debitTransaction = new Transaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                //TransactionNumber = // generated by the database via a sequence 
                CryptoProvider = CryptoProviders.None,
                CryptoAddress = "",
                TxnType = TxnTypes.DEBIT,
                TxnCode = TxnCodes.AFFILIATE_REDEEM,
                OriginalCurrencyCode = CurrencyCodes.NONE,
                OriginalAmount = 0,
                DestinationCurrencyCode = sourceAccount.CurrencyCode,
                DestinationAmount = amount,
                IsAmountKnown = true,
                Status = TxnStatus.Complete,
                AccountId = sourceAccount.AccountId,

                SourceAccountId = sourceAccount.AccountId,
                DestinationAccountId = destinationAccount.AccountId,
            };

            var creditTransaction = new Transaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                //TransactionNumber = // generated by the database via a sequence 
                CryptoProvider = CryptoProviders.None,
                CryptoAddress = "",
                TxnType = TxnTypes.DEBIT,
                TxnCode = TxnCodes.LOAD_DIRECT,
                OriginalCurrencyCode = CurrencyCodes.NONE,
                OriginalAmount = 0,
                DestinationCurrencyCode = destinationAccount.CurrencyCode,
                DestinationAmount = amount,
                IsAmountKnown = true,
                Status = TxnStatus.Complete,
                AccountId = destinationAccount.AccountId,

                SourceAccountId = sourceAccount.AccountId,
                DestinationAccountId = destinationAccount.AccountId,
            };

            await _ctx.Transactions.AddAsync(debitTransaction);
            await _ctx.Transactions.AddAsync(creditTransaction);

            await _ctx.SaveChangesAsync();

            // generate references --
            long num = Convert.ToInt64(debitTransaction.TransactionNumber);
            debitTransaction.Reference = _refGen.GenTxnRef(num);
            var groupRef = debitTransaction.TxnGroupRef = _refGen.GenTxnGroupRef(num);

            long num2 = Convert.ToInt64(creditTransaction.TransactionNumber);
            creditTransaction.Reference = _refGen.GenTxnRef(num2);
            creditTransaction.TxnGroupRef = groupRef;

            await _ctx.SaveChangesAsync();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAffiliateTransactions()
        {
            string userId = this.GetUserId();
            var accounts = _ctx.Accounts.Where(a => a.UserId == userId);

            var transactions = new List<Transaction>();
            foreach (var account in accounts)
            {
                var trAffiliate = _ctx.Transactions.Where(t => t.AccountId == account.AccountId && t.TxnCode == TxnCodes.AFFILIATE_FEE & t.TxnCode == TxnCodes.AFFILIATE_REDEEM);
                transactions.AddRange(trAffiliate);
            }

            return Json(transactions.OrderByDescending(txn => txn.DateCreated));
        }

    }
}
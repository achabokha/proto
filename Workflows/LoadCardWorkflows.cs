using Embily.Gateways;
using Embily.Messages;
using Embily.Models;
using Embily.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Embily.Workflows
{
    public class LoadCardWorkflows : BaseWorkflow
    {
        readonly ICardLoad _cardLoad;

        readonly IRefGen _refGen = new RefGenerator();

        const double MinLoadUSD = 50 - 20; // -5 for fees and rate decreases while in transit --
        const double MinLoadEUR = 50 - 20; // -5 for fees and rate decreases while in transit --

        public LoadCardWorkflows(ICardLoad cardLoad, NameValueCollection appSettings, EmbilyDbContext ctx, TextWriter log)
            : base(appSettings, ctx, log)
        {
            _cardLoad = cardLoad;
        }

        public async Task<NotifyEmail> Process(LoadCard msg)
        {
            var txnTransfer = await _ctx.Transactions.FindAsync(msg.TxnId);
            var txnGroupRef = txnTransfer.TxnGroupRef;
            var account = await _ctx.Accounts.FindAsync(txnTransfer.AccountId);
            var user = await _ctx.Users.FindAsync(account.UserId);

            if (IsInsufficientFunds(txnTransfer.DestinationCurrencyCode, msg.Amount))
            {
                await UpdateTxnStatus(txnTransfer.TransactionId, TxnStatus.InsufficientFunds);
                return CreateInsufficientFundsOutEmailMessage(msg, user, txnTransfer, account);
            }

            if (IsOverTheLimit(txnTransfer.DestinationCurrencyCode, msg.Amount))
            {
                await UpdateTxnStatus(txnTransfer.TransactionId, TxnStatus.OverTheLimit);
                return CreateOverTheLimitOutEmailMessage(msg, user, txnTransfer, account);
            }

            // calculate fees, if the first transaction - charge card purchase and shipping and handling 
            var handling = 0;
            var cardPurchase = 0;
            double affiliateFees = 0;

            bool isFirstTransaction = IsFirstTransaction(account);
            if (isFirstTransaction)
            {
                handling = CalcHandling(txnTransfer.DestinationCurrencyCode, user);
                cardPurchase = CalcCardPurchase(txnTransfer.DestinationCurrencyCode, user);
            }

            double amountBeforeLoad = txnTransfer.DestinationAmount - handling - cardPurchase;
            
            double platfromFees = CalcFees(user, amountBeforeLoad);

            // is affiliated? 
            Account affiliateAccount = await FindAffiliateAccountAsync(user, account);

            if (affiliateAccount != null)
            {
                affiliateFees = CalcAffiliateFees(affiliateAccount, amountBeforeLoad);
            }

            double amountToLoad = Math.Round(amountBeforeLoad - platfromFees, 2);

            Transaction txnLoad;
            try
            {
                // deposit to card using card load interface
                await _cardLoad.LoadCardAsync(account.ProviderUserId, account.ProviderAccountNumber, amountToLoad, txnTransfer.TransactionNumber);

                // create platform transactions 
                // 1. card purchase 
                if (handling != 0) await CreateTransaction(account, handling, TxnCodes.HANDLING, txnGroupRef);

                // 2. shipping and handling
                if (cardPurchase != 0) await CreateTransaction(account, cardPurchase, TxnCodes.CARD_PURCHASE, txnGroupRef);

                // 3. load_fees 
                if (platfromFees != 0) await CreateTransaction(account, platfromFees, TxnCodes.LOAD_FEE, txnGroupRef);

                // 4. load 
                txnLoad = await CreateTransaction(account, amountToLoad, TxnCodes.LOAD, txnGroupRef);

                // 5. affiliate fees 
                if (affiliateFees != 0) await CreateTransaction(affiliateAccount, affiliateFees, TxnCodes.AFFILIATE_FEE, txnGroupRef, type: TxnTypes.CREDIT);
            }
            catch (Exception ex)
            {
                LogError($"{ex.Message}\n{ex.StackTrace}");
                throw ex;
            }

            if (isFirstTransaction)
            {
                return CreateFirstTransactionOutEmailMessage(msg, user, txnLoad, account);
            }
            else
            {
                return CreateOutEmailMessage(msg, user, txnLoad, account);
            }
        }

        private double CalcAffiliateFees(Account affiliateAccount, double amountBeforeLoad)
        {
            double defaultRate = 0.75;
            double rate = 0;

            // account / rate 
            var fees = new Dictionary<string, double>
            {
                { "1000001021-0002", 0.75 },
                { "1000001021-0003", 0.75 }
            };

            fees.TryGetValue(affiliateAccount.AccountNumber, out rate);

            rate = rate == 0 ? defaultRate : rate;

            return Math.Round(amountBeforeLoad * rate / 100, 2);
        }

        private async Task<Account> FindAffiliateAccountAsync(ApplicationUser user, Account account)
        {
            if (string.IsNullOrWhiteSpace(user.AffiliatedWithUserId)) return null;

            var affiliatedWithAccount = await _ctx.Accounts.FirstOrDefaultAsync(
                a => a.UserId == user.AffiliatedWithUserId
                && a.CurrencyCode == account.CurrencyCode
                && a.AccountType == AccountTypes.Affiliate);

            return affiliatedWithAccount;
        }

        private bool IsFirstTransaction(Account account)
        {
            List<string> accountToCharge = new List<string>
            {
                "1000001073-0001", // Puput Novi
                "1000001071-0001",
                "1000001066-0001",
                "1000001064-0001",
                "1000001048-0001",
                "1000001061-0001",
                "1000001060-0001",
                "1000001060-0001",
                "1000001046-0002",
                "1000001050-0001",
                "1000001047-0001",
                "1000001042-0001",
            };

            var acct = accountToCharge.FirstOrDefault(a => a == account.AccountNumber);
            if (acct != null)
            {
                var count = _ctx.Transactions.Where(t => t.AccountId == account.AccountId && (t.TxnCode == TxnCodes.CARD_PURCHASE || t.TxnCode == TxnCodes.HANDLING)).Count();
                return count == 0;
            }

            return false;
        }

        private bool IsInsufficientFunds(CurrencyCodes currencyCode, double amount)
        {
            switch (currencyCode)
            {
                case CurrencyCodes.USD:
                    return amount < MinLoadUSD;
                case CurrencyCodes.EUR:
                    return amount < MinLoadEUR;
                default:
                    throw new ApplicationException($"Unknown/unsupported transaction code {currencyCode}");
            }
        }

        private bool IsOverTheLimit(CurrencyCodes currencyCode, double amount)
        {
            switch (currencyCode)
            {
                case CurrencyCodes.USD:
                    return amount > 5000;
                case CurrencyCodes.EUR:
                    return amount > 4000;
                default:
                    throw new ApplicationException($"Unknown/unsupported transaction code {currencyCode}");
            }
        }

        private int CalcCardPurchase(CurrencyCodes currencyCode, ApplicationUser user)
        {
            switch (currencyCode)
            {
                case CurrencyCodes.USD:
                    return 30;
                case CurrencyCodes.EUR:
                    return 20;
                default:
                    throw new ApplicationException($"Unknown/unsupported transaction code {currencyCode}");
            }
        }

        private int CalcHandling(CurrencyCodes currencyCode, ApplicationUser user)
        {
            switch (currencyCode)
            {
                case CurrencyCodes.USD:
                    return 50;
                case CurrencyCodes.EUR:
                    return 30; // grandfathered amount for 12 accounts on FirstTrasaction Purchase --
                default:
                    throw new ApplicationException($"Unknown/unsupported transaction code {currencyCode}");
            }
        }

        private double CalcFees(ApplicationUser user, double amount)
        {
            return Math.Round(amount * (2.75 + 0.75) / 100 + 0.50, 2); // load fees 2.75%, plus 0.75% per card, plus 50 c
        }

        async Task<Transaction> CreateTransaction(Account account, double amount, TxnCodes code, string txnGroupRef, TxnTypes type = TxnTypes.DEBIT, TxnStatus status = TxnStatus.Complete)
        {
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                //TransactionNumber = // generated by the database via a sequence 
                TxnGroupRef = txnGroupRef,
                CryptoProvider = CryptoProviders.None,
                CryptoAddress = "",
                TxnType = type,
                TxnCode = code,
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

            long num = Convert.ToInt64(transaction.TransactionNumber);
            transaction.Reference = _refGen.GenTxnRef(num);
            await _ctx.SaveChangesAsync();

            return transaction;
        }

        public NotifyEmail CreateOutEmailMessage(LoadCard msgIn, ApplicationUser user, Transaction txn, Account account)
        {
            var cardNumber = account.CardNumber;
            var msgOut = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Funds deposited",
                Message = null,
                Template = "topup-load",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}"},
                    { "currencyCode", txn.DestinationCurrencyCode.ToString() },
                    { "amount", txn.DestinationAmount.ToString("N2") },
                },

                TransactionNumber = msgIn.TransactionNumber,
                TxnId = msgIn.TxnId,
            };

            return msgOut;
        }

        public NotifyEmail CreateFirstTransactionOutEmailMessage(LoadCard msgIn, ApplicationUser user, Transaction txn, Account account)
        {
            var cardNumber = account.CardNumber;
            var msgOut = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "First transaction",
                Message = null,
                Template = "topup-first-transaction",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}"},
                    { "currencyCode", txn.DestinationCurrencyCode.ToString() },
                    { "amount", txn.DestinationAmount.ToString("N2") },
                },

                TransactionNumber = msgIn.TransactionNumber,
                TxnId = msgIn.TxnId,
            };

            return msgOut;
        }

        public NotifyEmail CreateInsufficientFundsOutEmailMessage(LoadCard msgIn, ApplicationUser user, Transaction txn, Account account)
        {
            var cardNumber = account.CardNumber;
            var msgOut = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Insufficient funds",
                Message = null,
                Template = "topup-insufficient-funds",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}"},
                    { "currencyCode", txn.DestinationCurrencyCode.ToString() },
                    { "amount", txn.DestinationAmount.ToString("N2") },
                },

                TransactionNumber = msgIn.TransactionNumber,
                TxnId = msgIn.TxnId,
            };

            return msgOut;
        }

        public NotifyEmail CreateOverTheLimitOutEmailMessage(LoadCard msgIn, ApplicationUser user, Transaction txn, Account account)
        {
            var cardNumber = account.CardNumber;
            var msgOut = new NotifyEmail
            {
                Name = $"{user.FirstName} {user.LastName}",
                To = user.Email,
                Subject = "Over the limit",
                Message = null,
                Template = "topup-over-the-limit",
                FieldDict = new Dictionary<string, string>
                {
                    { "firstName", user.FirstName},
                    { "lastName", user.LastName},
                    { "cardNumber", $"{cardNumber.Substring(0, 2)}**********{cardNumber.Substring(12)}"},
                    { "currencyCode", txn.DestinationCurrencyCode.ToString() },
                    { "amount", txn.DestinationAmount.ToString("N2") },
                },

                TransactionNumber = msgIn.TransactionNumber,
                TxnId = msgIn.TxnId,
            };

            return msgOut;
        }
    }
}

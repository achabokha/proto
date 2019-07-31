using Embily.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Services
{
    public class TransactionInfo
    {
        public DateTime TransactionDate { get; set; }

        public string TransactionCode { get; set; }

        public string Description { get; set; }

        public string TransactionCurrencyCode { get; set; }

        public decimal TransactionAmount { get; set; }

        public string BillingCurrencyCode { get; set; }

        public decimal BillingAmount { get; set; }

        public decimal RunningBalance { get; set; }

    }

    public interface ICardLoad
    {
        Task LoadCardAsync(string providerUserId, string providerAccountNumber, double amount, string transactionNumber);
    }

    public interface ICard
    {
        //Task<string> RegisterCardAsync(string ProviderAccountNumber);

        //Task<string> GetCardDetailsAsync(string providerUserId);

        Task<double> GetCardBalanceAsync(string providerUserId, string providerAccountNumber);

        Task<IList<TransactionInfo>> GetTransactionsAsync(string providerUserId, string providerAccountNumber);
    }
}

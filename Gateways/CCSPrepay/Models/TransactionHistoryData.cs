using System;
using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class TransactionHistoryData
    {
        
        public long ClientId { get; set; }

        public string TransactionDate { get; set; }

        public string TransactionCode { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal BillingAmount { get; set; }

        public string  BillingCurrencyCode { get; set; }

        public decimal RunningBalance { get; set; }

        public string CardAccNameAddress { get; set; }
    }
}
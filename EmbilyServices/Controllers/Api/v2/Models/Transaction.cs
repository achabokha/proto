using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices.Controllers.Api.v2.Models
{
    public class Transaction
    {
        public string TransacitonId { get; set; }

        public string ReferenceNumber { get; set; }

        public string TransactionType { get; set; }

        public string Date { get; set; }

        public string CurrencyCode { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }
    }
}

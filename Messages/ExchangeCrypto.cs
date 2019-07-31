using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Messages
{
    public class ExchangeCrypto : BaseMessage 
    {
        public double OriginalAmount { get; set; }

        public string OriginalCurrencyCode { get; set; }

        public string DestinationCurrencyCode { get; set; }
    }
}

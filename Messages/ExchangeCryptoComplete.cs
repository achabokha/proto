using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Messages
{
    public class ExchangeCryptoComplete : BaseMessage 
    {
        public long OrderId { get; set; }

        public string DestinationCurrencyCode { get; set; }

        public string OriginalCurrencyCode { get; set; }
    }
}

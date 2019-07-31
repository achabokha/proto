using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Messages
{
    public class DetectTransfer : BaseMessage
    {
        public string CryptoTxnId { get; set; }

        public string OriginalCurrencyCode { get; set; }

        public string DestinationCurrencyCode { get; set; }

        public DateTimeOffset AprxTxnDatetime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Messages
{
    public class LoadCard : BaseMessage
    {
        public double Amount { get; set; }

        public string CurrencyCode { get; set; }

        public double ExchangeFee { get; set; }
    }
}

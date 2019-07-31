using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CoinPayInTh.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RequestPaymentReceived
    {
        [JsonProperty("addresses")]
        public List<string> Addresses { get; set; }
    }
}
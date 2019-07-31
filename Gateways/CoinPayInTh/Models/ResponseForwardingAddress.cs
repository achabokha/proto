using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CoinPayInTh.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ResponseForwardingAddress
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("addresses")]
        public Dictionary<string, CoinAddress> Addresses { get; set; }
    }
}
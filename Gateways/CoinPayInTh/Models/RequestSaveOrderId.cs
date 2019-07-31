using System;
using System.Collections.Generic;

using System.Globalization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Embily.Gateways.CoinPayInTh.Models
{
    public partial class RequestSaveOrderId
    {
        [JsonProperty("addresses")]
        public List<string> Addresses { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }
    }  
}

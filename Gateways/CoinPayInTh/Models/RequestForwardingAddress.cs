using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Embily.Gateways.CoinPayInTh.Models
{

    public class RequestForwardingAddress
    {
        [JsonProperty("bxid")]
        public string BxId { get; set; } 

        [JsonProperty("callback")]
        public string Callback { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; } = "0.00";

        [JsonProperty("currency_from")]
        public string CurrencyFrom { get; set; }

        [JsonProperty("currency_to")]
        public string CurrencyTo { get; set; }

        [JsonProperty("order_label")]
        public string OrderLabel { get; set; } = "Card Purchase";
    }

}


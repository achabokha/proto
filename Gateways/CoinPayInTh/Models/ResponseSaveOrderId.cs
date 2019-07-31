using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CoinPayInTh.Models
{
    public partial class ResponseSaveOrderId
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("data")]
        public object[] Data { get; set; }
    }
}

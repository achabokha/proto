using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CoinPayInTh.Models
{
    public partial class ResponseCallbacks
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("confirmed_in_full")]
        public bool ConfirmedInFull { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}

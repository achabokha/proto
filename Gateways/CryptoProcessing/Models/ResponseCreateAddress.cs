using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CryptoProcessing.Models
{
    public partial class ResponseCreateAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

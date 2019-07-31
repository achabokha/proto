using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CryptoProcessing.Models
{
    public partial class RequestCreateAdress
    {
        [JsonProperty("name")]
        public string Name { get; set; } //Account 1000010001-0002
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CryptoProcessing.Models
{
    public partial class GetAddressesResponse
    {        
            [JsonProperty("addresses")]
            public List<CryptoAddress> Addresses { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; } 
    }
}

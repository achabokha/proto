using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class Dutiable
    {
        [JsonProperty("DeclaredCurrency")]
        public string DeclaredCurrency { get; set; }

        [JsonProperty("DeclaredValue")]
        public string DeclaredValue { get; set; }
    }
}

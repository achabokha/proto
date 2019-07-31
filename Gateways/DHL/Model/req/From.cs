using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class From
    {
        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("Postalcode")]
        public string Postalcode { get; set; }
    }
}

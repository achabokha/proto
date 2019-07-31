using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class GetQuote
    {
        [JsonProperty("Request")]
        public Request Request { get; set; }

        [JsonProperty("From")]
        public From From { get; set; }

        [JsonProperty("BkgDetails")]
        public BkgDetails BkgDetails { get; set; }

        [JsonProperty("To")]
        public From To { get; set; }
    }
}

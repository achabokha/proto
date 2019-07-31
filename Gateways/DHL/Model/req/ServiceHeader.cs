using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class ServiceHeader
    {
        [JsonProperty("MessageTime")]
        public DateTimeOffset MessageTime { get; set; }

        [JsonProperty("MessageReference")]
        public string MessageReference { get; set; }

        [JsonProperty("SiteID")]
        public string SiteId { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}

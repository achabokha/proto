using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class PDctRequest
    {
        [JsonProperty("@xmlns:p")]
        public string XmlnsP { get; set; }

        [JsonProperty("@xmlns:p1")]
        public string XmlnsP1 { get; set; }

        [JsonProperty("@xmlns:p2")]
        public string XmlnsP2 { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string XmlnsXsi { get; set; }

        [JsonProperty("@xsi:schemaLocation")]
        public string XsiSchemaLocation { get; set; }

        [JsonProperty("GetQuote")]
        public GetQuote GetQuote { get; set; }
    }
}

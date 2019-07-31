using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class QtdShp
    {
        [JsonProperty("GlobalProductCode")]
        public string GlobalProductCode { get; set; }

        [JsonProperty("LocalProductCode")]
        public string LocalProductCode { get; set; }

        [JsonProperty("QtdShpExChrg")]
        public QtdShpExChrg QtdShpExChrg { get; set; }
    }
}

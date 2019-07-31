using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class QtdShpExChrg
    {
        [JsonProperty("SpecialServiceType")]
        public string SpecialServiceType { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class Request
    {
        [JsonProperty("ServiceHeader")]
        public ServiceHeader ServiceHeader { get; set; }
    }
}

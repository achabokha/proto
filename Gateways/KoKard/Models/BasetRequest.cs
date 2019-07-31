using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.KoKard.Models
{
    public class BaseRequest
    {
        [JsonProperty("APIID")]
        public string APIID { get; set; }

        [JsonProperty("APIKey")]
        public string APIKey { get; set; }
    }
}

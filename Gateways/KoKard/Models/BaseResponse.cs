using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.KoKard.Models
{
    public class BaseResponse
    {
        [JsonProperty("StatusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("ErrorDetails")]
        public object ErrorDetails { get; set; }
    }
}

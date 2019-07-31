using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class LoadCardResponse : BaseResponse
    {
        [JsonProperty("response")]
        public BaseResponseImpl Response { get; set; }

        public override void EnsureSuccess()
        {
            EnsureSuccessImpl(Response);
        }
    }
}

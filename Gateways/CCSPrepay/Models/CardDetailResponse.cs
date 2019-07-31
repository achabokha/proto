using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class CardDetailResponseImpl : BaseResponseImpl
    {
        [JsonProperty("data")]
        public CardDetailData Data { get; set; }
    }

    public class CardDetailResponse : BaseResponse
    {
        [JsonProperty("response")]
        public CardDetailResponseImpl Response { get; set; }

        public override void EnsureSuccess()
        {
            EnsureSuccessImpl(Response);
        }
    }
}


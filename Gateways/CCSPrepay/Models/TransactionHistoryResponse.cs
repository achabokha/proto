using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class TransactionHistoryResponseImpl : BaseResponseImpl
    {
        [JsonProperty("data")]
        public List<TransactionHistoryData> Data { get; set; }
    }

    public class TransactionHistoryResponse : BaseResponse
    {
        [JsonProperty("response")]
        public TransactionHistoryResponseImpl Response { get; set; }

        public override void EnsureSuccess()
        {
            EnsureSuccessImpl(Response);
        }
    }
}

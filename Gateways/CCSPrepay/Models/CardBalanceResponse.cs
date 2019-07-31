using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class CardBalanceResponseImpl : BaseResponseImpl
    {
        [JsonProperty("data")]
        public CardBalanceData Data { get; set; }
    }

    public class CardBalanceResponse : BaseResponse
    {
        [JsonProperty("response")]
        public CardBalanceResponseImpl Response { get; set; }

        public override void EnsureSuccess()
        {
            EnsureSuccessImpl(Response);
        }
    }
}
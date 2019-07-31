using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class UpgradeCardholderResponse : BaseResponse
    {
        [JsonProperty("response")]
        public BaseResponseImpl Response { get; set; }

        public override void EnsureSuccess()
        {
            EnsureSuccessImpl(Response);
        }
    }
}
using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class UpdateProfileCardResponseImpl : BaseResponseImpl
    {
        [JsonProperty("data")]
        public UpdateProfileCardData Data { get; set; }
    }

    public class UpdateProfileCardResponse : BaseResponse
    {
        [JsonProperty("response")]
        public UpdateProfileCardResponseImpl Response { get; set; }

        public override void EnsureSuccess()
        {
            EnsureSuccessImpl(Response);
        }
    }
}
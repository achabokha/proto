using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class BaseRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class BaseAuthRequest : BaseRequest
    {
        [JsonProperty("accesstoken")]
        public string AccessToken { get; set; }
    }
}

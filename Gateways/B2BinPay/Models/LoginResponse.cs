using Newtonsoft.Json;

namespace Embily.Gateways.B2BinPay.Models
{
    public class LoginResponse
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("lifetime")]
        public string LifeTime { get; set; }
    }
}

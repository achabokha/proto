using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class CardDetailRequest : BaseAuthRequest
    {
        [JsonProperty("client_id")]
        public long ClientId { get; set; }
    }
}
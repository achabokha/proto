using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class CardBalanceRequest : BaseAuthRequest
    {
        [JsonProperty("client_id")]
        public long ClientId { get; set; }
    }
}
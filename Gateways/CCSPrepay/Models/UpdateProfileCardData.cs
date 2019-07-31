using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class UpdateProfileCardData
    {
        [JsonProperty("clientId")]
        public long ClientId { get; set; }
    }
}
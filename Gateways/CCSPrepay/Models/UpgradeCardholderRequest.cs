using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class UpgradeCardholderRequest : BaseRequest
    {
        [JsonProperty("client_id")]
        public long ClientId { get; set; }

        // undocumented --
        [JsonProperty("client_email")]
        public string ClientEmail { get; set; }

        [JsonProperty("idproofimg")]
        public string IdProofImg { get; set; }

        [JsonProperty("addressproofimg")]
        public string AddressProofImg { get; set; }

    }
}
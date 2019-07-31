using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class LoadCardRequest : BaseAuthRequest
    {
        [JsonProperty("client_id")]
        public long ClientId { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        /// <summary>
        /// External transaction ID
        /// </summary>
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }
    }
}
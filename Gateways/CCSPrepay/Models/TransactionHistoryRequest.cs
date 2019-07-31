using Newtonsoft.Json;

namespace Embily.Gateways.CCSPrepay.Models
{
    public class TransactionHistoryRequest : BaseAuthRequest
    {
        [JsonProperty("clientID")]
        public long ClientId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int PageNumber { get; set; }

    }
}
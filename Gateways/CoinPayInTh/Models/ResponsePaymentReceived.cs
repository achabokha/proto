using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CoinPayInTh.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ResponsePaymentReceived
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("paid")]
        public Paid[] Paid { get; set; }

        [JsonProperty("paid_by")]
        public PaidBy PaidBy { get; set; }

        [JsonProperty("is_enough")]
        public bool IsEnough { get; set; }

        [JsonProperty("payment_received")]
        public bool PaymentReceived { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    public partial class Paid
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("cryptocurrency")]
        public string Cryptocurrency { get; set; }

        [JsonProperty("proof_link")]
        public string ProofLink { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public partial class PaidBy
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("proof_link")]
        public string ProofLink { get; set; }
    }
}
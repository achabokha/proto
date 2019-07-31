using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Embily.Gateways.Blockcypher.Models
{

    ///
    /// see fields description here:
    /// https://www.blockcypher.com/dev/bitcoin/?shell#paymentforward
    ///
    public class PaymentForward
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("input_address")]
        public string InputAddress { get; set; }

        [JsonProperty("process_fees_address")]
        public string ProcessFeesAddress { get; set; }

        [JsonProperty("process_fees_satoshis")]
        public Satoshi ProcessFeesSatoshis { get; set; }

        [JsonProperty("process_fees_percent")]
        public double ProcessFeesPercent { get; set; }

        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }

        [JsonProperty("enable_confirmations")]
        public bool EnableConfirmations{ get; set; }

        [JsonProperty("mining_fees_satoshis")]
        public Satoshi MiningFeesSatoshis { get; set; }

        [JsonProperty("txs")]
        public IList<string> Txs { get; set; }
    }
}
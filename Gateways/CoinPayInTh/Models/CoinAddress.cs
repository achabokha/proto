using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CoinPayInTh.Models
{
    public partial class CoinAddress
    {
        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("address")]
        public string CryptoAddress { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("exchange_rate")]
        public double ExchangeRate { get; set; }

        [JsonProperty("payment_url")]
        public string PaymentUrl { get; set; }

        [JsonProperty("qr_code_base64")]
        public string QrCodeBase64 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

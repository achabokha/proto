using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.B2BinPay.Models
{
    public class CreatePaymentOrderRequest
    {
        //public class CreatePaymentOrderRequestData
        //{
        [JsonProperty("wallet")]
        public string Wallet { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("lifetime")]
        public string LifeTime { get; set; }

        [JsonProperty("pow")]
        public string Pow { get; set; }

        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }
        //}

        //[JsonProperty("data")]
        //public CreatePaymentOrderRequestData Data { get; set; }

    }
}

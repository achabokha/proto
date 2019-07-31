using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class BkgDetails
    {
        [JsonProperty("PaymentCountryCode")]
        public string PaymentCountryCode { get; set; }

        [JsonProperty("Date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("ReadyTime")]
        public string ReadyTime { get; set; }

        [JsonProperty("ReadyTimeGMTOffset")]
        public string ReadyTimeGMTOffset { get; set; }

        [JsonProperty("DimensionUnit")]
        public string DimensionUnit { get; set; }

        [JsonProperty("WeightUnit")]
        public string WeightUnit { get; set; }

        [JsonProperty("Pieces")]
        public Pieces Pieces { get; set; }

        [JsonProperty("PaymentAccountNumber")]
        public string PaymentAccountNumber { get; set; }

        [JsonProperty("IsDutiable")]
        public string IsDutiable { get; set; }
    }
}

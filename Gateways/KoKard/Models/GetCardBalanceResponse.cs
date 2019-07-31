﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Embily.Gateways.KoKard.Models
{
    public class CardBalanceTableItem
    {

        [JsonProperty("CardReferenceID")]
        public string CardReferenceID { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Amount")]
        public double Amount { get; set; }
    }

    public class CardBalanceTableResult
    {

        [JsonProperty("CardBalanceTable")]
        public CardBalanceTableItem[] CardBalanceTable { get; set; }
    }

    public class GetCardBalanceResponse : BaseResponse
    {
        [JsonProperty("AdditionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }

        [JsonProperty("TableResult")]
        public CardBalanceTableResult TableResult { get; set; }
    }

}
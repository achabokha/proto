﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Embily.Gateways.KoKard.Models
{

    public class AddCardHolderDocumentsRequest : BaseRequest
    {
        [JsonProperty("CardHolderID")]
        public string CardHolderID { get; set; }

        [JsonProperty("Documents")]
        public Document[] Documents { get; set; }
    }

}
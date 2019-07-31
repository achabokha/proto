using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class Pieces
    {
        [JsonProperty("Piece")]
        public Piece Piece { get; set; }
    }
}

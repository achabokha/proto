using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.DHL.Model.req
{
    public partial class Piece
    {
        [JsonProperty("PieceID")]
        public string PieceId { get; set; }

        [JsonProperty("Height")]
        public string Height { get; set; }

        [JsonProperty("Depth")]
        public string Depth { get; set; }

        [JsonProperty("Width")]
        public string Width { get; set; }

        [JsonProperty("Weight")]
        public string Weight { get; set; }
    }
}

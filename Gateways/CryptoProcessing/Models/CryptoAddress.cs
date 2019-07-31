using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways.CryptoProcessing.Models
{
    public partial class CryptoAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("final_balance")]
        public long FinalBalance { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("n_tx")]
        public long NTx { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("total_received")]
        public long TotalReceived { get; set; }

        [JsonProperty("total_sent")]
        public long TotalSent { get; set; }

        [JsonProperty("txs")]
        public List<Tx> Txs { get; set; }
    }
}

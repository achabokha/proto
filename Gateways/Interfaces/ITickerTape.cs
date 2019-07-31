using System.Collections.Generic;
using System.Threading.Tasks;

namespace Embily.Gateways
{
    public class Ticker
    {
        public string symbol { get; set; }

        public decimal btc { get; set; }

        public decimal usd { get; set; }

        public decimal eur { get; set; }

        public decimal thb { get; set; }
    }

    public interface ITickerTape
    {
        Task<List<Ticker>> GetTickers(); 
    }
}

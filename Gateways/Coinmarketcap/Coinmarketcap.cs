using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Gateways
{
    public class Coinmarketcap : ITickerTape
    {
        readonly string url = "https://api.coinmarketcap.com/v1/ticker";



        public async Task<List<Ticker>> GetTickers()
        {
            var tape = GetCurrenciesOfIntrest();

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(url + "?convert=EUR");
                var tickers = JsonConvert.DeserializeObject<CoinmarketcapResponse[]>(response);

                UpdateBTCUSDEUR(tape, tickers);

                response = await client.GetStringAsync(url + "?convert=THB");
                var tickersThb = JsonConvert.DeserializeObject<CoinmarketcapResponse[]>(response);

                UpdateTHB(tape, tickersThb);

                return tape;
            }
        }

        private void UpdateBTCUSDEUR(List<Ticker> tape, CoinmarketcapResponse[] tickers)
        {
            foreach(var tip in tape)
            {
                var tick = tickers.FirstOrDefault(t => t.Symbol == tip.symbol.ToUpper());
                tip.btc = ParsePrice(tick.PriceBtc);
                tip.usd = ParsePrice(tick.PriceUsd);
                tip.eur = ParsePrice(tick.PriceEur);
            }
        }

        private void UpdateTHB(List<Ticker> tape, CoinmarketcapResponse[] tickers)
        {
            foreach (var tip in tape)
            {
                var tick = tickers.FirstOrDefault(t => t.Symbol == tip.symbol.ToUpper());
                tip.thb = ParsePrice(tick.PriceThb);
            }
        }
 
        private List<Ticker> GetCurrenciesOfIntrest()
        {
            // TODO: refactor to use strong typing

            var result = new List<Ticker> {
                new Ticker
                {
                    symbol = "btc",
                },
                new Ticker
                {
                    symbol = "eth",
                },
                new Ticker
                {
                    symbol = "ltc",
                },
                new Ticker
                {
                    symbol = "xrp",
                },
                new Ticker
                {
                    symbol = "xmr",
                },
                new Ticker
                {
                    symbol = "dgb",
                },
            };

            return result;
        }

        static decimal ParsePrice(string price)
        {
            return Decimal.Parse(price.ToString(), System.Globalization.NumberStyles.Float);
        }
    }
}

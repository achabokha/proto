using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using BitfinexApi;
using Newtonsoft.Json;
using System.Linq;
using Embily.Gateways.Extensions;

namespace Embily.Gateways
{
    public class BitfinexExchange : ICryptoExchange
    {
        private BitfinexApiV1 _api;

        public BitfinexExchange()
        {
            string key = Environment.GetEnvironmentVariable("BitfinexApi_key");
            string secret = Environment.GetEnvironmentVariable("BitfinexApi_secret");

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(secret))
            {
                _api = new BitfinexApiV1(key, secret);
            }
        }

        // possible alternative for jobs with Config files
        //public Bitfinex(string key, string secret)
        //{
        //    if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(secret))
        //    {
        //        _api = new BitfinexApiV1(key, secret);
        //    }
        //}

        public async Task<string> GetNewCryptoAddress(CryptoCurrencyCodes code)
        {
            if (_api == null)
            {
                return $"fake {Guid.NewGuid().ToString()} fake";
            }

            var request = new DepositRequest
            {
                Method = GetBitfixenMethod(code),
                WalletName = "exchange",
                Renew = 1,
            };

            var response = await _api.DepositAsync(request);

            return response.Address;
        }

        public async Task<double> GetTransfer(string cryptoTxnId, CryptoCurrencyCodes code, DateTimeOffset aprxTxnDatetime)
        {
            var request = new HistoryRequest
            {
                Currency = code.ToString(),
                Method = GetBitfixenMethod(code),
                Since = aprxTxnDatetime.AddDays(-1).ToUnixTimeSeconds().ToString(),
                Until = aprxTxnDatetime.AddDays(1).ToUnixTimeSeconds().ToString(),
                Limit = 1000,
            };

            LogRequest(request);

            var response = await _api.HistoryAsync(request);

            LogResponse(response);

            var item = response.FirstOrDefault(t => t.Txid == cryptoTxnId);

            if (item == null)
            {
                throw new CryptoTransferHasNotArrivedYetException($"Crypto Txn Hash: {cryptoTxnId}, {code}");
            }

            return Convert.ToDouble(item.Amount);
        }

        private static string GetBitfixenMethod(CryptoCurrencyCodes code)
        {
            switch (code)
            {
                case CryptoCurrencyCodes.BTC:
                    return "bitcoin";
                case CryptoCurrencyCodes.LTC:
                    return "litecoin";
                case CryptoCurrencyCodes.ETH:
                    return "ethereum";
                default:
                    throw new ApplicationException($"unsupported crypto currency code {code}");
            }
        }

        public async Task<long> ExchangeCryptoOrderAsync(double amount, CryptoOrderSymbols orderSymbol)
        {
            var request = new NewOrderRequest
            {
                Symbol = orderSymbol.ToString().ParseEnum<OrderSymbols>(),
                Amount = amount.ToString(),
                // Price to buy or sell at. Must be positive. Use random number for market orders.
                Price = new Random().NextDouble().ToString(),
                Side = OrderSides.Sell,
                Type = OrderTypes.ExchangeMarket,
            };

            LogRequest(request);

            var response = await _api.NewOrderAsync(request);

            LogResponse(response);

            return response.OrderId;
        }

        public async Task<ExchangeResult> GetExchangeCryptoTradeAsync(long orderId, CryptoOrderSymbols symbol)
        {
            ExchangeResult er = null;
            var orderSymbol = symbol.ToString().ParseEnum<OrderSymbols>();
            Retry.Do(() =>
            {
                er = CheckTradeComplete(orderId, orderSymbol).Result;
            }, TimeSpan.FromSeconds(5), 5);

            if (er == null)
            {
                throw new ExchangeCryptoTradeException("Order is still being executed.");
            }

            er.ExchangeFee = await GetExchangeFees(orderId, orderSymbol);

            return er;
        }

        async Task<double> GetExchangeFees(long orderId, OrderSymbols symbol)
        {
            var request = new PastTradesRequest
            {
                Symbol = symbol.ToString(),
                Timestamp = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds().ToString(),
                Until = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds().ToString(),
                LimitTrades = 100,
                Reverse = 0,
            };

            LogRequest(request);

            var response = await _api.PastTradesAsync(request);

            LogResponse(response);

            var trades = response.Where(t => t.OrderId == orderId);
            double feeAmount = trades.Sum(t => Convert.ToDouble(t.FeeAmount));

            if (trades == null)
            {
                // exception will make trigger one more attempt to execute this method --
                throw new ApplicationException("Trades are not found, unable to get the exchange fees");
            }

            return Math.Abs(feeAmount);
        }

        async Task<ExchangeResult> CheckTradeComplete(long orderId, OrderSymbols symbol)
        {
            var request = new OrderStatusRequest
            {
                OrderId = orderId
            };

            LogRequest(request);

            var response = await _api.OrderStatusAsync(request);

            LogResponse(response);

            if (response.IsLive)
            {
                throw new ApplicationException("Order is still being executed. Waiting for completion.");
            }

            if (response.IsCancelled)
            {
                throw new ApplicationException("Order is canceled. Something went wrong.");
            }

            double amount = Convert.ToDouble(response.AvgExecutionPrice) * Convert.ToDouble(response.ExecutedAmount);
            return new ExchangeResult
            {
                DestinationAmount = amount,
                ExchangeFee = 0, // bitfinex fee in '-' amount
            };
        }

        private static void LogRequest(BaseRequest request)
        {
            Console.WriteLine($"Request: {request}");
            Console.WriteLine($"Request: {JsonConvert.SerializeObject(request, Formatting.Indented)}");
        }

        private static void LogResponse(object response)
        {
            Console.WriteLine($"Response: {response}");
            Console.WriteLine($"Response: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
        }

    }
}

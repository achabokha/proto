
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Embily.Gateways.Blockcypher.Models;
using Newtonsoft.Json;

namespace Embily.Gateways.Blockcypher
{

    public enum BlockcypherCurrencyCode
    {
        BTC,
        LTC,
        ETH
    }

    public class BlockcypherAPI
    {
        readonly bool _consoleLogging;
        readonly string _token;
        readonly string _callbackUrl;

        public BlockcypherAPI(string token, string callbackUrl)
        {
            _token = token;
            _callbackUrl = callbackUrl;
            _consoleLogging = true;
        }

        private string GetEndpointBaseAddress(BlockcypherCurrencyCode currencyCode)
        {
            switch(currencyCode)
            {
                case BlockcypherCurrencyCode.BTC:
                    return "https://api.blockcypher.com/v1/btc/main";
                case BlockcypherCurrencyCode.LTC:
                    return "https://api.blockcypher.com/v1/ltc/main";
                case BlockcypherCurrencyCode.ETH:
                    return "https://api.blockcypher.com/v1/eth/main";
                default:
                    throw new ApplicationException("Unsupported currency code");
            }
        }

        private string GetDestAddr(BlockcypherCurrencyCode currencyCode)
        {
            switch (currencyCode)
            {
                case BlockcypherCurrencyCode.BTC:
                    //return "1LLxMZQ1uBuqmKtymj7XCSZyHEufDELLkg"; // A's test address --
                    return "1ATjaksof8nbPknUDpEoE6MsLfxbpynLMt";
                case BlockcypherCurrencyCode.LTC:
                    return "LcE7ivpA9MmWqU1PSr4VG67msvtrsgepSQ";
                    //return "LcE7ivpA9MmWqU1PSr4VG67msvtrsgepSQ";
                case BlockcypherCurrencyCode.ETH:
                    return "0x5b34b439a1e7c52a39ea53fbf0a50d193cffce19";
                default:
                    throw new ApplicationException("Unsupported currency code");
            }
        }

        public async Task<PaymentForward> CreatePaymentEndpoint(BlockcypherCurrencyCode currencyCode)
        {
            string destAddr = GetDestAddr(currencyCode);
            string endpointBaseAddress = GetEndpointBaseAddress(currencyCode);

            var r = await PostRequestAsync<PaymentForward>(
                $"{endpointBaseAddress}/payments", 
                new PaymentForward
                {
                    Destination = destAddr,
                    CallbackUrl = _callbackUrl,
                });

            return r;
        }

        public async Task<PaymentForward[]> ListPaymentEndpoint(BlockcypherCurrencyCode currencyCode)
        {
            string endpointBaseAddress = GetEndpointBaseAddress(currencyCode);
            return await GetRequestAsyncA<PaymentForward>($"{endpointBaseAddress}/payments");
        }

        public async Task DeletePaymentEndpoint(BlockcypherCurrencyCode currencyCode, string id)
        {
            string endpointBaseAddress = GetEndpointBaseAddress(currencyCode);
            await DeleteRequestAsync($"{endpointBaseAddress}/payments", id);
        }

        private async Task<T> PostRequestAsync<T>(string endpoint, object request)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{endpoint}?token={_token}";
                var content = new StringContent(JsonConvert.SerializeObject(request));

                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseRaw = await response.Content.ReadAsStringAsync();

                if (_consoleLogging)
                {
                    Console.WriteLine($"Response Body Raw:");
                    Console.WriteLine($"{responseRaw}");
                }

                return JsonConvert.DeserializeObject<T>(responseRaw);
            }
        }

        private async Task<T[]> GetRequestAsyncA<T>(string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{endpoint}?token={_token}";

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseRaw = await response.Content.ReadAsStringAsync();

                if (_consoleLogging)
                {
                    Console.WriteLine($"Response Body Raw:");
                    Console.WriteLine($"{responseRaw}");
                }
                return JsonConvert.DeserializeObject<T[]>(responseRaw);
            }
        }

        private async Task DeleteRequestAsync(string endpoint, string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{endpoint}/{id}?token={_token}";

                var response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                var responseRaw = await response.Content.ReadAsStringAsync();

                if (_consoleLogging)
                {
                    Console.WriteLine($"Response Body Raw:");
                    Console.WriteLine($"{responseRaw}");
                }
            }
        }
    }
}


/*  

curl -d '{"destination":"15qx9ug952GWGTNn7Uiv6vode4RcGrRemh","callback_url": "https://my.domain.com/callbacks/new-pay"}' https://api.blockcypher.com/v1/btc/main/payments?token=YOURTOKEN

{
"input_address": "16uKw7GsQSzfMaVTcT7tpFQkd7Rh9qcXWX",
"destination": "15qx9ug952GWGTNn7Uiv6vode4RcGrRemh",
"callback_url": "https://my.domain.com/callbacks/payments",
"id": "399d0923-e920-48ee-8928-2051cbfbc369",
"token": "YOURTOKEN"
}

*/

using Embily.Gateways.B2BinPay.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Gateways.B2BinPay
{
    public class B2BinPayAPI
    {
        // test env 
        //readonly string _apiUrl = "https://paysystemtest.b2broker.info";
        //readonly string _key = "55b9f5c433";
        //readonly string _secret = "970451d7a54f9ac";


        // Prod env
        readonly string _apiUrl = "https://btc.b2binpay.com";

        readonly string _key = "1747481bfe";
        readonly string _secret = "8a7e2d842f2c910";

        readonly IB2BinPayWebAPI _api;

        public B2BinPayAPI()
        {
            var httpClient = new HttpClient(new HttpLoggingHandler(/*new NativeMessageHandler()*/)) { BaseAddress = new Uri(_apiUrl) };
            _api = RestService.For<IB2BinPayWebAPI>(httpClient);
        }

        public async Task<CreatePaymentOrderResponse> CreatePaymentOrderAsync(long amount)
        {
            var login = await this.LoginAsync();

            string auth = "Bearer " + login.AccessToken;

            var request = new CreatePaymentOrderRequest
            {   
                Wallet = "397",
                Amount = amount.ToString(),
                LifeTime = "0",
                Pow = "8",
                CallbackUrl = "https://services.embily.com/callbacks/B2BinPayCallback",
                
            };

            var r = await _api.CreatePaymentOrderAsync(auth, request);
            return r;
        }

        public async Task<LoginResponse> LoginAsync()
        {
            string auth = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_key}:{_secret}"));

            var token = await _api.LoginAsync(auth);
            return token;
        }

    }
}

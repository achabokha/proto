using Embily.Gateways.CoinPayInTh.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Embily.Gateways.CoinPayInTh
{
    public class CoinPayInThAPI
    {
        const string _url = "https://api.coinpay.co.th/";
        const string _bxId = "5f58c877653f";

        // webhook link: https://webhook.site/#/7fbdf705-8e3a-4229-bca6-cf4390a9f5dd/2272eb90-4e27-4b13-b295-03f2197a2279/0
        // callbock url: "https://webhook.site/7fbdf705-8e3a-4229-bca6-cf4390a9f5dd"

        readonly string _callbackUrl = "https://services.embily.com/callbacks/CoinPayCallback";

        readonly IServiceWebApi _api;

        public CoinPayInThAPI()
        {
            _api = ServiceWebApiFactory.CreateWebApi(_url);
        }

        public CoinPayInThAPI(string callbackUrl)
        {
            _callbackUrl = callbackUrl;
            _api = ServiceWebApiFactory.CreateWebApi(_url);
        }

        /// <summary>
        /// Request Forwarding Address, also crypto amount, etc.
        /// </summary>
        /// <param name="sourceCurrencyCode"></param>
        /// <param name="amount"></param>
        /// <param name="destinationCurrencyCode"></param>
        /// <returns></returns>
        public async Task<CoinAddress> GetForwardingAddress(string sourceCurrencyCode, string amount, string destinationCurrencyCode)//RequestForwardingAddress request)
        {
            var response = await _api.GetForwardingAddress(new RequestForwardingAddress()
            {
                BxId = _bxId,
                Amount = amount,
                CurrencyFrom = sourceCurrencyCode,
                CurrencyTo = destinationCurrencyCode,
                Callback = _callbackUrl,
            });

            var coinaddress = new CoinAddress();
            response.Data.Addresses.TryGetValue(destinationCurrencyCode, out coinaddress);

            return coinaddress;
        }

        /// <summary>
        /// Check transaction is created
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponsePaymentReceived> CheckTransactionIsCreated(RequestPaymentReceived request)
        {
            return await _api.CheckTransactionIsCreated(request);
        }

        /// <summary>
        /// Save Order ID, in another words create a callback URL
        /// </summary>
        /// <param name="coinAddress"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ResponseSaveOrderId> SaveOrderId(string coinAddress, string orderId)
        {
            var request = new RequestSaveOrderId();

            request.Addresses = new List<string>();
            request.Addresses.Add(coinAddress);
            request.OrderId = orderId;

            var response = await _api.SaveOrderId(request);

            return response;
        }
    }
}

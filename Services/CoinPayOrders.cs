using Embily.Gateways.CoinPayInTh;
using Embily.Gateways.CoinPayInTh.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Embily.Services
{
    public class CoinPayOrders : ICardOrder
    {
        readonly CoinPayInThAPI _api;
        private IConfiguration _configuration;

        public CoinPayOrders(IConfiguration configuration)
        {
            _configuration = configuration;

            var callbackUrl = configuration["CoinPayInTh_callbackUrl"];
            _api = new CoinPayInThAPI(callbackUrl);
        }

        public CoinPayOrders()
        {
             _api = new CoinPayInThAPI();
        }

        public CoinPayOrders(string callbackUrl)
        {
            _api = new CoinPayInThAPI(callbackUrl);
        }

        public async Task<CardOrderDetails> GetForwardingAddress(double amount, string currencyCode, string cryptoCurrencyCode)
        {
            CardOrderDetails cardOrder = new CardOrderDetails();
            // 1. get address from CoinPay with details on crypto amount 
            CoinAddress forwardingAddress = await _api.GetForwardingAddress(currencyCode, amount.ToString(), cryptoCurrencyCode);

            if (forwardingAddress != null)
            {
                cardOrder.CryptoAddress = forwardingAddress.CryptoAddress;
                cardOrder.Amount = double.Parse(forwardingAddress.Amount, CultureInfo.InvariantCulture);
                cardOrder.QrCodeSrc = forwardingAddress.QrCodeBase64;
            }
            else
            {
                throw new ApplicationException($"Unable to receive forwarding address from CoinPay");
            }

            return cardOrder;
        }

        public async Task CreateCardOrder(string orderId, string cryptoAddress)
        {
            // 2. set orderId in CoinPay for callback (callback is based on orderId)
            var setorder = await _api.SaveOrderId(cryptoAddress, orderId);

            if(!setorder.Success)
            {
                new ApplicationException($"Unable to create a callback in CoinPay for OrderId ${orderId} and Crypto Address ${cryptoAddress}");
            }
        }
    }
}

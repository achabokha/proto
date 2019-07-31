using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Embily.Gateways;
using Microsoft.Extensions.Configuration;

using Embily.Models;
using Embily.Gateways.Blockcypher;
using Embily.Gateways.CryptoProcessing;
using Embily.Gateways.CryptoProcessing.Models;

namespace Embily.Services
{
    public class CryptoAddressCreator : ICryptoAddress
    {
        readonly IConfiguration _config;
        readonly BlockcypherAPI _blockcypherAPI;
        readonly CryptoProcessingAPI _cryptoProcessingAPI;

        public CryptoAddressCreator(IConfiguration config)
        {
            _config = config;

            var blockcypherToken = _config["Blockcypher_token"];
            var blockcypherCallbackURL = _config["Blockcypher_callbackUrl"];

            _blockcypherAPI = new BlockcypherAPI(blockcypherToken, blockcypherCallbackURL);

            // accountID controls where callback will go. Callback sets up via CryptoProcessing API -- 
            var accountId = _config["CryptoProcessing_accountId"];
            _cryptoProcessingAPI = new CryptoProcessingAPI(accountId);
        }

        public async Task<string> GetNewAddressAsync(CurrencyCodes code, string accountNumber)
        {
            switch (code)
            {
                case CurrencyCodes.BTC:
                case CurrencyCodes.LTC:
                    return await GetBlockcypherAddressAsync(code);
                case CurrencyCodes.ETH:
                    return await GetCryptoProcessingAddressAsync(code, accountNumber);
                default:
                    throw new ApplicationException($"Unsupported Currency Code {code}");
            }
        }

        private async Task<string> GetCryptoProcessingAddressAsync(CurrencyCodes code, string accountNumber)
        {
            RequestCreateAdress requestCreateAdress = new RequestCreateAdress()
            {
                Name = accountNumber
            };

            var address = await _cryptoProcessingAPI.CreateAddress(requestCreateAdress);
            return address.Address;
        }

        public async Task<string> GetBlockcypherAddressAsync(CurrencyCodes code)
        {
            var currencCode = code.ToString().ParseEnum<BlockcypherCurrencyCode>();
            var r = await _blockcypherAPI.CreatePaymentEndpoint(currencCode);
            return r.InputAddress;
        }
    }
}

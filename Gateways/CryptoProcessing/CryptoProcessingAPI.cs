using Embily.Gateways.CryptoProcessing.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Gateways.CryptoProcessing
{
    public class CryptoProcessingAPI
    {
        const string _url = "https://eth.mainnet.backend.cryptoprocessing.io/";
        const string _token = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE1MzE4MzkzODYsInN1YiI6ImQ5YjI1ZDg2LTUwZTMtNDllMy04OWYxLTFhZTAyMzlmNzM0OCJ9.xUX3vc2BTNtRN9qEf4nG2RBYJXLsOoVB-OX30f9VB0g";
        const string _blockchainType = "eth";
        readonly string _account_id;

        readonly IServiceCryptoProcessingWebApi _api;

        public CryptoProcessingAPI(string accountId)
        {
            _account_id = accountId;
            _api = ServiceCryptoProcessingWebApiFactory.CreateWebApi(_url);
        }

        /// <summary>
        /// GetAddresses
        /// </summary>
        /// <returns></returns>
        public async Task<GetAddressesResponse> GetAddresses()
        {
            var response = await _api.GetAddresses(_blockchainType, _account_id, _token);

            return response;
        }

        /// <summary>
        /// Create Address
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseCreateAddress> CreateAddress(RequestCreateAdress request)
        {
            var response = await _api.CreateAddress(_blockchainType, _account_id, _token, request);

            return response;
        }
    }
}

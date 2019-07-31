using Embily.Gateways.CryptoProcessing.Models;
using Refit;
using System.Threading.Tasks;

namespace Embily.Gateways
{
    public interface IServiceCryptoProcessingWebApi
    {
        /// <summary>
        /// CreateAddresss
        /// </summary>
        /// <returns></returns>
        [Get("/api/v1/{blockchainType}/accounts/{account_id}/addresses")]
        Task<GetAddressesResponse> GetAddresses(string blockchainType, string account_id, [Header("Authorization")] string bearerToken);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/v1/{blockchainType}/accounts/{account_id}/addresses")]
        Task<ResponseCreateAddress> CreateAddress(string blockchainType, string account_id, [Header("Authorization")] string bearerToken, [Body(BodySerializationMethod.Json)] RequestCreateAdress request);
    }
}

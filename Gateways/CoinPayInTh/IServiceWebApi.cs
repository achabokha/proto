using Embily.Gateways.CoinPayInTh.Models;
using Embily.Gateways.DHL.Model;
using Refit;
using System.Threading.Tasks;

namespace Embily.Gateways
{
    public interface IServiceWebApi
    {
        /// <summary>
        /// Request forwarding addresses
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/get_forwarding_address")]
        Task<ResponseForwardingAddress> GetForwardingAddress([Body(BodySerializationMethod.Json)] RequestForwardingAddress request);

        /// <summary>
        /// Check transaction is created
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/payment_received")]
        Task<ResponsePaymentReceived> CheckTransactionIsCreated([Body(BodySerializationMethod.Json)] RequestPaymentReceived request);

        /// <summary>
        /// Save order id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/save_order_id")]
        Task<ResponseSaveOrderId> SaveOrderId([Body(BodySerializationMethod.Json)] RequestSaveOrderId request);
    }
}

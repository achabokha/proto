using Embily.Gateways.B2BinPay.Models;
using Refit;
using System.Threading.Tasks;


namespace Embily.Gateways.B2BinPay
{
    public interface IB2BinPayWebAPI
    {
       
        [Get("/api/login")]
        Task<LoginResponse> LoginAsync([Header("Authorization")] string authorization);

        [Post("/api/v1/pay/bills")]
        Task<CreatePaymentOrderResponse> CreatePaymentOrderAsync([Header("Authorization")] string authorization, [Body(BodySerializationMethod.Json)] CreatePaymentOrderRequest request);

    }
}

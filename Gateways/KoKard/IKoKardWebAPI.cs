using Embily.Gateways.KoKard.Models;
using Refit;
using System.Threading.Tasks;

namespace Embily.Gateways.KoKard
{
    internal interface IKoKardWebAPI
    {
        [Post("/api/Inquiry/GetCardBalance")]
        Task<GetCardBalanceResponse> GetCardBalanceAsync([Body(BodySerializationMethod.Json)] GetCardBalanceRequest request);

        [Post("/api/Inquiry/GetTransactionHistory")]
        Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync([Body(BodySerializationMethod.Json)] GetTransactionHistoryRequest request);

        [Post("/api/Financial/CreditFunds")]
        Task<CreditFundsResponse> CreditFundsAsync([Body(BodySerializationMethod.Json)] CreditFundsRequest request);

        [Post("/api/Financial/DebitFunds")]
        Task<DebitFundsResponse> DebitFundsAsync([Body(BodySerializationMethod.Json)] DebitFundsRequest request);

        [Post("/api/Financial/Reversal")]
        Task<ReversalResponse> ReversalAsync([Body(BodySerializationMethod.Json)] ReversalRequest request);

        [Post("/api/Financial/ShareFunds")]
        Task<ShareFundsResponse> ShareFundsAsync([Body(BodySerializationMethod.Json)] ShareFundsRequest request);

        [Post("/api/Card/RegisterCardholder")]
        Task<RegisterCardholderResponse> RegisterCardholderAsync([Body(BodySerializationMethod.Json)] RegisterCardholderRequest request);

        [Post("/api/Card/GetStatus")]
        Task<GetCardStatusResponse> GetCardStatusAsync([Body(BodySerializationMethod.Json)] GetCardStatusRequest request);

        [Post("/api/Card/SetStatus")]
        Task<SetCardStatusResponse> SetCardStatusAsync([Body(BodySerializationMethod.Json)] SetCardStatusRequest request);

        [Post("/api/Card/AddCardholderCard")]
        Task<BaseResponse> AddCardholderCardAsync([Body(BodySerializationMethod.Json)] AddCardholderCardRequest request);

        [Post("/api/Card/Verify")]
        Task<VerifyCardResponse> VerifyCardAsync([Body(BodySerializationMethod.Json)] VerifyCardRequest request);

        [Post("/api/Card/AddCardHolderDocuments")]
        Task<BaseResponse> AddCardHolderDocumentsAsync([Body(BodySerializationMethod.Json)] AddCardHolderDocumentsRequest request);

        [Post("/api/Card/SetPIN")]
        Task<SetCardPINResponse> SetCardPINAsync([Body(BodySerializationMethod.Json)] SetCardPINRequest request);

        [Post("/api/Card/ResetPIN")]
        Task<ResetCardPINResponse> ResetCardPINAsync([Body(BodySerializationMethod.Json)] ResetCardPINRequest request);

        [Post("/api/Card/ValidatePIN")]
        Task<ValidateCardPINResponse> ValidateCardPINAsync([Body(BodySerializationMethod.Json)] ValidateCardPINRequest request);

        [Post("/api/Card/GetCardholderProfile")]
        Task<GetCardholderProfileResponse> GetCardholderProfileAsync([Body(BodySerializationMethod.Json)] GetCardholderProfileRequest request);

        [Post("/api/Card/UpdateProfile")]
        Task<UpdateProfileResponse> UpdateProfileAsync([Body(BodySerializationMethod.Json)] UpdateProfileRequest request);

        [Post("/api/Card/Activate")]
        Task<ActivateCardResponse> ActivateCardAsync([Body(BodySerializationMethod.Json)] ActivateCardRequest request);

        [Post("/api/Inquiry/GetMiniStatement")]
        Task<GetMiniStatementResponse> GetMiniStatementAsync([Body(BodySerializationMethod.Json)] GetMiniStatementRequest request);

        [Post("/api/Inquiry/GetExchangeRate")]
        Task<GetExchangeRateResponse> GetExchangeRateAsync([Body(BodySerializationMethod.Json)] GetExchangeRateRequest request);

        [Post("/api/Inquiry/ValidateActivation")]
        Task<ValidateActivationResponse> ValidateActivationAsync([Body(BodySerializationMethod.Json)] ValidateActivationRequest request);

        [Post("/api/Inquiry/SearchCustomer")]
        Task<SearchCustomerResponse> SearchCustomerAsync([Body(BodySerializationMethod.Json)] SearchCustomerRequest request);

        [Post("/api/Inquiry/GetProgramInfo")]
        Task<GetProgramInfoResponse> GetProgramInfoAsync([Body(BodySerializationMethod.Json)] GetProgramInfoRequest request);
    }
}
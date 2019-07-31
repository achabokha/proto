using Embily.Gateways.KoKard.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Embily.Gateways.KoKard
{
    public class KoKardAPI
    {
        // Sandbox credentials --
        readonly string _apiUrl = "http://k2kodiak-sandbox.com";
        readonly string _apiId = "3mB117i0";
        readonly string _apiKey = "1x373Mb";

        readonly IKoKardWebAPI _api;

        public KoKardAPI()
        {
            // using logging --
            var httpClient = new HttpClient(new HttpLoggingHandler(/*new NativeMessageHandler()*/)) { BaseAddress = new Uri(_apiUrl) };
            _api = RestService.For<IKoKardWebAPI>(httpClient);
        }

        public KoKardAPI(string url, string apiId, string apiKey) 
        {
            _apiUrl = url;
            _apiId = apiId;
            _apiKey = apiKey;

            // using logging --
            var httpClient = new HttpClient(new HttpLoggingHandler(/*new NativeMessageHandler()*/)) { BaseAddress = new Uri(_apiUrl) };
            _api = RestService.For<IKoKardWebAPI>(httpClient);
        }

        public async Task<RegisterCardholderResponse> RegisterCardholderAsync(RegisterCardholderRequest request)
        {
            this.SetCredentials(request);

            var response = await _api.RegisterCardholderAsync(request);

            this.EnsureSuccess(response); // does not seem relevant, refit checks for 500 status and blows error before this check -- 

            return response;
        }

        public async Task<UpdateProfileResponse> UpdateProfileAsync(UpdateProfileRequest request)
        {
            this.SetCredentials(request);

            var response = await _api.UpdateProfileAsync(request);

            this.EnsureSuccess(response); // does not seem relevant, refit checks for 500 status and blows error before this check -- 

            return response;
        }

        /*
            1 Driver's License Driver's License
            2 Passport Passport
            3 ID Card ID Card
            4 Matricula Consular Matricula Consular
            5 IFE Voter Card IFE Voter Card
            6 Military ID Military ID
            7 State ID/Government Work Card State ID/Government Work Card         
        */
        public async Task<BaseResponse> AddCardHolderDocumentsAsync(string cardHolderID, Document[] documents)
        {
            var request = new AddCardHolderDocumentsRequest
            {
                CardHolderID = cardHolderID,
                Documents = documents,
            };

            this.SetCredentials(request);

            var response = await _api.AddCardHolderDocumentsAsync(request);

            this.EnsureSuccess(response); // does not seem relevant -- 

            return response;
        }

        public async Task<BaseResponse> AddCardholderCardAsync(string cardHolderID, string cardReferenceID)
        {
            var request = new AddCardholderCardRequest
            {
                CardHolderID = cardHolderID,
                CardReferenceID = cardReferenceID,
                CardStatusID = "B", // B - all transactions allowed --
            };

            this.SetCredentials(request);

            var response = await _api.AddCardholderCardAsync(request);

            this.EnsureSuccess(response); // does not seem relevant -- 

            return response;
        }

        public async Task<GetCardholderProfileResponse> GetCardholderProfileAsync(string cardHolderID)
        {
            var request = new GetCardholderProfileRequest
            {
                CardHolderID = cardHolderID,
            };

            this.SetCredentials(request);

            var response = await _api.GetCardholderProfileAsync(request);

            this.EnsureSuccess(response); // does not seem relevant -- 

            return response;
        }

        /// <summary>
        /// aka Validate Activation
        /// </summary>
        /// <param name="cardReferenceID"></param>
        /// <returns></returns>
        public async Task<VerifyCardResponse> VerifyCardAsync(string cardReferenceID)
        {
            var request = new VerifyCardRequest
            {
                CardReferenceID = cardReferenceID
            };

            this.SetCredentials(request);

            var response = await _api.VerifyCardAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<GetCardStatusResponse> GetCardStatusAsync(string cardReferenceID)
        {
            var request = new GetCardStatusRequest
            {
                CardReferenceID = cardReferenceID
            };

            this.SetCredentials(request);

            var response = await _api.GetCardStatusAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<SetCardStatusResponse> SetCardStatusAsync(string cardReferenceID, string statusCode)
        {
            var request = new SetCardStatusRequest
            {
                CardReferenceID = cardReferenceID,
                StatusCode  = statusCode
            };

            this.SetCredentials(request);

            var response = await _api.SetCardStatusAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<GetCardBalanceResponse> GetCardBalanceAsync(string cardReferenceID)
        {
            var request = new GetCardBalanceRequest
            {
                CardReferenceID = cardReferenceID
            };

            this.SetCredentials(request);

            var response = await _api.GetCardBalanceAsync(request);

            this.EnsureSuccess(response);

            return response;       
        }

        public async Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(string cardReferenceID)
        {
            var request = new GetTransactionHistoryRequest
            {
                CardReferenceID = cardReferenceID,
                DateFrom = DateTime.UtcNow.AddDays(-89).ToString("MM/dd/yyyy"),
                DateTo = DateTime.UtcNow.AddDays(1).ToString("MM/dd/yyyy"),
            };

            this.SetCredentials(request); 

            var response = await _api.GetTransactionHistoryAsync(request);

            this.EnsureSuccess(response); 

            return response;
        }

        public async Task<GetMiniStatementResponse> GetMiniStatementAsync(string cardReferenceID, int numOfTxns)
        {
            var request = new GetMiniStatementRequest
            {
                CardReferenceID = cardReferenceID,
                NumberOfTrans = numOfTxns.ToString(),
            };

            this.SetCredentials(request);

            var response = await _api.GetMiniStatementAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<GetExchangeRateResponse> GetExchangeRateAsync(string fromCurrency, string toCurrency, double amount)
        {
            var request = new GetExchangeRateRequest
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Amount = String.Format("{0:0.00}", amount)
            };

            this.SetCredentials(request);

            var response = await _api.GetExchangeRateAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<CreditFundsResponse> CreditFundsAsync(string cardReferenceID, double amount)
        {
            var request = new CreditFundsRequest
            {
                CardReferenceID = cardReferenceID,
                Amount = String.Format("{0:0.00}", amount)
            };

            this.SetCredentials(request);

            var response = await _api.CreditFundsAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<DebitFundsResponse> DebitFundsAsync(string cardReferenceID, double amount)
        {
            var request = new DebitFundsRequest
            {
                CardReferenceID = cardReferenceID,
                Amount = String.Format("{0:0.00}", amount)
            };

            this.SetCredentials(request);

            var response = await _api.DebitFundsAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        /// <summary>
        /// The reversal performs a reversal of the specified transaction for the specified card
        /// </summary>
        /// <param name="cardReferenceID"></param>
        /// <param name="transactionID"></param>
        /// <returns></returns>
        public async Task<ReversalResponse> ReversalAsync(string cardReferenceID, string transactionID)
        {
            var request = new ReversalRequest
            {
                CardReferenceID = cardReferenceID,
                TransactionID = transactionID,
            };

            this.SetCredentials(request);

            var response = await _api.ReversalAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<ShareFundsResponse> ShareFundsAsync(string fromCardReferenceID, string toCardReferenceID, double amount)
        {
            var request = new ShareFundsRequest
            {
                FromCardReferenceID = fromCardReferenceID,
                ToCardReferenceID = toCardReferenceID,
                Amount = String.Format("{0:0.00}", amount)
            };

            this.SetCredentials(request);

            var response = await _api.ShareFundsAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<SetCardPINResponse> SetCardPINAsync(string cardReferenceID, string newPIN)
        {
            var request = new SetCardPINRequest
            {
                CardReferenceID = cardReferenceID,
                NewPIN = newPIN,
            };

            this.SetCredentials(request);

            var response = await _api.SetCardPINAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<ResetCardPINResponse> ResetCardPINAsync(string cardReferenceID)
        {
            var request = new ResetCardPINRequest
            {
                CardReferenceID = cardReferenceID,
            };

            this.SetCredentials(request);

            var response = await _api.ResetCardPINAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task <ValidateCardPINResponse> ValidateCardPINAsync(string cardReferenceID, string pin)
        {
            var request = new ValidateCardPINRequest
            {
                CardReferenceID = cardReferenceID,
                PIN = pin
            };

            this.SetCredentials(request);

            var response = await _api.ValidateCardPINAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<ActivateCardResponse> ActivateCardAsync(string cardReferenceID, double amount)
        {
            var request = new ActivateCardRequest
            {
                CardReferenceID = cardReferenceID,
                Amount = String.Format("{0:0.00}", amount)
            };

            this.SetCredentials(request);

            var response = await _api.ActivateCardAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<ValidateActivationResponse> ValidateActivationAsync(string cardReferenceID)
        {
            var request = new ValidateActivationRequest
            {
                CardReferenceID = cardReferenceID,
            };

            this.SetCredentials(request);

            var response = await _api.ValidateActivationAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<SearchCustomerResponse> SearchCustomerAsync(string firstName, string lastName)
        {
            var request = new SearchCustomerRequest
            {
                //CardReferenceID = cardReferenceID,
                Firstname = firstName,
                Lastname = lastName,
            };

            this.SetCredentials(request);

            var response = await _api.SearchCustomerAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        public async Task<GetProgramInfoResponse> GetProgramInfoAsync(string programCode)
        {
            var request = new GetProgramInfoRequest
            {
                ProgramCode = programCode
            };

            this.SetCredentials(request);

            var response = await _api.GetProgramInfoAsync(request);

            this.EnsureSuccess(response);

            return response;
        }

        void SetCredentials(BaseRequest request)
        {
            request.APIID = _apiId;
            request.APIKey = _apiKey;
        }

        private void EnsureSuccess(BaseResponse response)
        {
            if(response.StatusCode != 200)
            {
                // TODO: capture an error messages and pass to the exception --
                throw new KoKardAPIException();
            }
        }
    }
}

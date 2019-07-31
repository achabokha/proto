using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Embily.Gateways;
using Embily.Gateways.CCSPrepay.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Embily.Gateways.CCSPrepay
{
    public class CCSPrepayAPI
    {
        string _endpointBaseAddress = "https://www.ccsprepay.com/ccsportal/api";

        TextWriter _log;

        readonly string _username;

        readonly string _password;

        static string _accessToken;

        string AccessToken
        {
            get
            {
                if (DateTimeOffset.UtcNow > _accessTokenExpires)
                {
                    var r = this.GetTokenAsync().Result;
                    _accessToken = r.Response.Data.AccessToken;
                    _accessTokenExpires = DateTimeOffset.Parse(r.Response.Data.Expires);
                }
                return _accessToken;
            }
        }
        static DateTimeOffset _accessTokenExpires = DateTimeOffset.MinValue;

        public CCSPrepayAPI(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public CCSPrepayAPI()
        {
            _username = Environment.GetEnvironmentVariable("CCSPrepay_username");
            _password = Environment.GetEnvironmentVariable("CCSPrepay_password");
        }

        public async Task<GetTokenResponse> GetTokenAsync()
        {
            return await ExecuteRequest<GetTokenResponse>(new GetTokenRequest(), "gettoken");
        }

        public async Task<LoadCardResponse> LoadCardAsync(LoadCardRequest request)
        {
            return await ExecuteAuthRequest<LoadCardResponse>(request, "Loadcard");
        }

        public async Task<CardDetailResponse> CardDetailAsync(CardDetailRequest request)
        {
            return await ExecuteAuthRequest<CardDetailResponse>(request, "Carddetail");
        }

        public async Task<CardBalanceResponse> CardBalanceAsync(CardBalanceRequest request)
        {
            return await ExecuteAuthRequest<CardBalanceResponse>(request, "Cardbalance");
        }

        public async Task<UpdateProfileCardResponse> UpdateProfileCardAsync(UpdateProfileCardRequest request)
        {
            return await ExecuteAuthRequest<UpdateProfileCardResponse>(request, "Updateprofilecard ");
        }

        public async Task<UpgradeCardholderResponse> UpgradeCardholderAsync(UpgradeCardholderRequest request)
        {
            return await ExecuteRequest<UpgradeCardholderResponse>(request, "UpgradeCardholder");
        }

        public async Task<TransactionHistoryResponse> TransactionHistoryAsync(TransactionHistoryRequest request)
        {
            return await ExecuteAuthRequest<TransactionHistoryResponse>(request, "transactionhistory");
        }

        async Task<T> ExecuteAuthRequest<T>(BaseAuthRequest request, string endpoint) where T : BaseResponse, new()
        {
            request.AccessToken = AccessToken;
            return await ExecuteRequest<T>(request, endpoint);
        }

        async Task<T> ExecuteRequest<T>(BaseRequest request, string endpoint) where T : BaseResponse, new()
        {
            //Log($"not validating certificate", _log);
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            request.Username = _username;
            request.Password = _password;
            
            string url = $"{_endpointBaseAddress}/{endpoint}";

            var urlencodedContent = CreateUrlenocdedContent(request);
            var contentEncoded = new StringContent(urlencodedContent, null, "application/x-www-form-urlencoded");

            using (HttpClient client = new HttpClient())
            {
                Log($"Request>>>", _log);
                Log($"url: {url}", _log);
                Log($"headers: {contentEncoded.Headers.ToString()}", _log);
                Log($"content: {contentEncoded.ReadAsStringAsync().Result}", _log);

                var response = await client.PostAsync(url, contentEncoded);

                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();

                Log($"Response:", _log);
                Log($"{responseJson}", _log);

                var r = JsonConvert.DeserializeObject<T>(responseJson);
                r.EnsureSuccess();
                return r;
            }
        }

        // keeping here as reference, multipart form --
        // async Task<T> ExecuteRequestMultiPart<T>(UpgradeCardholderRequest request, string endpoint, byte[] id, byte[] address) where T : BaseResponse, new()
        // {
        //     request.Username = _username;
        //     request.Password = _password;

        //     string url = $"{_endpointBaseAddress}/{endpoint}";

        //     var content = new MultipartFormDataContent();

        //     var keyValues = new List<KeyValuePair<string, string>>();
        //     keyValues.Add(new KeyValuePair<string, string>("username", _username));
        //     keyValues.Add(new KeyValuePair<string, string>("password", _password));
        //     keyValues.Add(new KeyValuePair<string, string>("accesstoken", _accessToken));
        //     keyValues.Add(new KeyValuePair<string, string>("client_id", request.ClientId.ToString()));
        //     keyValues.Add(new KeyValuePair<string, string>("client_email", request.ClientEmail));
        //     // keyValues.Add(new KeyValuePair<string, string>("idproofimg", "id"));
        //     // keyValues.Add(new KeyValuePair<string, string>("addressproofimg", "address"));

        //     content.Add(new FormUrlEncodedContent(keyValues));

        //     content.Add(new StreamContent(new MemoryStream(id)), "idproofimg", "id.txt");
        //     content.Add(new StreamContent(new MemoryStream(address)), "addressproofimg", "address.txt");

        //     using(HttpClient client = new HttpClient())
        //     {
        //         Log($"Request>>>", _log);
        //         Log($"url: {url}", _log);
        //         Log($"headers: {content.Headers.ToString()}", _log);
        //         Log($"content: {content.ReadAsStringAsync().Result}", _log);

        //         var response = await client.PostAsync(url, content);

        //         response.EnsureSuccessStatusCode();

        //         var responseJson = await response.Content.ReadAsStringAsync();

        //         Log($"Response:", _log);
        //         Log($"{responseJson}", _log);

        //         var r = JsonConvert.DeserializeObject<T>(responseJson);
        //         r.EnsureSuccess();
        //         return r;
        //     }
        // }


        // keeping here as a reference, no Json.net serialization, KeyValue pairs --
        // async Task<T> ExecuteRequestA<T>(UpgradeCardholderRequest request, string endpoint) where T : BaseResponse, new()
        // {
        //     request.Username = _username;
        //     request.Password = _password;

        //     string url = $"{_endpointBaseAddress}/{endpoint}";

        //     var keyValues = new List<KeyValuePair<string, string>>();

        //     keyValues.Add(new KeyValuePair<string, string>("username", _username));
        //     keyValues.Add(new KeyValuePair<string, string>("password", _password));
        //     keyValues.Add(new KeyValuePair<string, string>("accesstoken", _accessToken));
        //     keyValues.Add(new KeyValuePair<string, string>("client_id", request.ClientId.ToString()));
        //     keyValues.Add(new KeyValuePair<string, string>("client_email", request.ClientEmail));
        //     keyValues.Add(new KeyValuePair<string, string>("idproofimg", request.IdProofImg));
        //     keyValues.Add(new KeyValuePair<string, string>("addressproofimg", request.IdProofImg));

        //     var content = new FormUrlEncodedContent(keyValues);

        //     using(HttpClient client = new HttpClient())
        //     {
        //         Log($"Request>>>", _log);
        //         Log($"url: {url}", _log);
        //         Log($"headers: {content.Headers.ToString()}", _log);
        //         Log($"content: {content.ReadAsStringAsync().Result}", _log);

        //         var response = await client.PostAsync(url, content);

        //         response.EnsureSuccessStatusCode();

        //         var responseJson = await response.Content.ReadAsStringAsync();

        //         Log($"Response:", _log);
        //         Log($"{responseJson}", _log);

        //         var r = JsonConvert.DeserializeObject<T>(responseJson);
        //         r.EnsureSuccess();
        //         return r;
        //     }
        // }

        string CreateUrlenocdedContent(BaseRequest request)
        {
            StringWriter sw = new StringWriter();
            using (var writer = new UrlencodeJsonWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } };
                serializer.Serialize(writer, request);
            }
            return sw.ToString().Substring(1);
        }

        void Log(string msg, TextWriter log)
        {
            Console.WriteLine(msg);
            //_log.WriteLine (msg);
        }

        public void EncodingTest()
        {
            // see for more: 
            // http://ronaldrosiernet.azurewebsites.net/Blog/2013/12/07/posting_urlencoded_key_values_with_httpclient
            // 

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("date", "22/09/1971"),
            });

            // var myHttpClient = new HttpClient();
            // var response = await myHttpClient.PostAsync(uri.ToString(), formContent);
        }
    }

}
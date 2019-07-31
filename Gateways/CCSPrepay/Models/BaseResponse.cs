using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Embily.Gateways.CCSPrepay.Models
{
    public enum ResponseStatus
    {
        Fail,
        Error,
        Success,
    }

    public class BaseResponseImpl
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseStatus Status { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }
    }

    public abstract class BaseResponse
    {
        public abstract void EnsureSuccess();

        public void EnsureSuccessImpl(BaseResponseImpl response)
        {
            if (response.Status != ResponseStatus.Success)
            {
                throw new ApplicationException($"{response.StatusMessage}");
            }
        }
    }
}

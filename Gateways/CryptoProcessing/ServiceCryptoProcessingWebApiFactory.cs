using Refit;

namespace Embily.Gateways
{
    public static class ServiceCryptoProcessingWebApiFactory
    {
        public static IServiceCryptoProcessingWebApi CreateWebApi(string serverUrl)
        {
            return RestService.For<IServiceCryptoProcessingWebApi>(serverUrl, new RefitSettings
            {
                //AuthorizationHeaderValueGetter = "",
                HttpMessageHandlerFactory = () => new WebApiHandler(loggingEnabled: true)
            });
        }
    }
}

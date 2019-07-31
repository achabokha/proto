using Refit;

namespace Embily.Gateways
{
    public static class ServiceWebApiFactory
    {
        public static IServiceWebApi CreateWebApi(string serverUrl)
        {
            return RestService.For<IServiceWebApi>(serverUrl, new RefitSettings
            {
                HttpMessageHandlerFactory = () => new WebApiHandler(loggingEnabled: true)
            });
        }
    }
}

using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Module1.Client
{
    public class Module1ClientFactory
    {
        public static IModule1Client Create(
            Module1ClientConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<IModule1Client> logger)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = configuration.ServiceUrl;
            return new Module1Client(logger, httpClient);
        }
    }
}
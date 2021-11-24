using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Module1.Client
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddModule1Client(
            this IServiceCollection services,
            Module1ClientConfiguration configuration)
        {
            return services.AddSingleton(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<IModule1Client>>();
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                return Module1ClientFactory.Create(configuration, httpClientFactory, logger);
            });
        }

        public static Module1ClientConfiguration ToModule1ClientConfiguration(
            this IConfiguration configuration,
            string serviceUrl = "ServiceDiscovery:Module1:Url")
        {
            Uri url;
            try
            {
                url = new Uri(configuration.EnsureKey(serviceUrl));
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cannot create valid service URL", ex);
            }

            return new Module1ClientConfiguration {ServiceUrl = url};
        }


        private static string EnsureKey(this IConfiguration configuration, string key)
        {
            var value = configuration[key];
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Configuration does not contain key '{key}'");
            }

            return value;
        }
    }
}
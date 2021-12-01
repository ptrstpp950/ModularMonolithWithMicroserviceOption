using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bootloader.Contracts;
using Microsoft.Extensions.Logging;
using Module1.DataContracts;
using RestEase;

namespace Module1.Client
{
    public interface IModule1ClientRest
    {
        [Get("WeatherForecast")]
        Task<IEnumerable<WeatherForecast>> GetWeatherForecast([Query] int numberOfDays);
    }

    public interface IModule1Client
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecast(int numberOfDays);
        Task<bool> CheckHealthCheck();
    }

    public class Module1Client : IModule1Client
    {
        private readonly ILogger _logger;
        private readonly HttpClient _healthHttpClient;
        private readonly IModule1ClientRest _restClient;

        internal Module1Client(ILogger<IModule1Client> logger, HttpClient httpClient, HttpClient healthHttpClient)
        {
            _logger = logger;
            _healthHttpClient = healthHttpClient;
            _restClient =
                new RestClient(httpClient)
                    {
                        JsonSerializerSettings = BootloaderJsonSerializerDefaults.DefaultSerializerSettings
                    }
                    .For<IModule1ClientRest>();
        }
        public Task<IEnumerable<WeatherForecast>> GetWeatherForecast(int numberOfDays)
        {
            return _restClient.GetWeatherForecast(numberOfDays);
        }

        public async Task<bool> CheckHealthCheck()
        {
            var result = await _healthHttpClient.GetAsync("/healthz/live");
            return result.StatusCode == HttpStatusCode.OK;
        }
    }
}
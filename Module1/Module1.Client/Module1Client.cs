using System;
using System.Collections.Generic;
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
    }

    public class Module1Client : IModule1Client
    {
        private readonly ILogger _logger;
        private readonly IModule1ClientRest _restClient;

        internal Module1Client(ILogger<IModule1Client> logger, HttpClient httpClient)
        {
            _logger = logger;
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
    }
}
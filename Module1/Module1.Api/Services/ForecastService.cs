using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Module1.DataContracts;

namespace ModuleApi1.Services
{
    public interface IForecastService
    {
        public Task<IEnumerable<WeatherForecast>> GetWeatherForNextDays(int numberOfDays);
    }

    public class ForecastService : IForecastService
    {
        private readonly ILogger _logger;

        public ForecastService(ILogger<ForecastService> logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<WeatherForecast>> GetWeatherForNextDays(int numberOfDays)
        {
            if (numberOfDays < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfDays), "Number of days should be positive");
            }

            _logger.LogInformation("Calculating weather forecast for the next {numberOfDays}", numberOfDays);

            var rng = new Random();
            var summaryList = Enum.GetValues(typeof(Summary)).Cast<Summary>().ToList();

            var forecastList =
                Enumerable
                    .Range(1, numberOfDays)
                    .Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = summaryList[rng.Next(summaryList.Count)]
                    });

            return Task.FromResult(forecastList);
        }
    }
}
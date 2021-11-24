using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Module1.Client;

namespace Module2.Api.Controllers
{
    [ApiController]
    [Route("module2/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IModule1Client _module1Client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IModule1Client module1Client)
        {
            _logger = logger;
            _module1Client = module1Client;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var days = rng.Next(1, 10);
            var forecast = await _module1Client.GetWeatherForecast(days);
            return forecast.Select(x => new WeatherForecast()
            {
                Date = x.Date,
                Summary = "External response is: " + x.Summary,
                TemperatureC = x.TemperatureC
            });
        }
    }
}
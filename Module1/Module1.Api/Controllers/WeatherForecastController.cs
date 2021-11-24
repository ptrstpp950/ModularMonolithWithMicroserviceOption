using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Module1.DataContracts;
using ModuleApi1.Services;

namespace ModuleApi1.Controllers
{
    [ApiController]
    [Route("module1/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IForecastService _forecastService;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get([FromQuery] int numberOfDays)
        {
            return Ok(await _forecastService.GetWeatherForNextDays(numberOfDays));
        }
    }
}
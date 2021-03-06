using System;

namespace Module1.DataContracts
{
    public enum Summary
    {
        Freezing,
        Bracing,
        Chilly,
        Cool,
        Mild,
        Warm,
        Balmy,
        Hot,
        Sweltering,
        Scorching
    }

    public class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public Summary Summary { get; set; }
    }
}
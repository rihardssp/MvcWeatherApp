using System;

namespace Services.HttpServices.Abstractions.Weather
{
    public class WeatherApiConsumerConfiguration : IWeatherApiConsumerConfiguration
    {
        public const string Section = "WeatherApiConfiguration";
        public string ApiId { get; set; }
        public string ApiUrl { get; set; }
    }
}

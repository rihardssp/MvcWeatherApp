using Services.Enums;
using System;

namespace Services.HttpServices.Abstractions.Weather
{
    public class WeatherApiConsumerConfiguration : IWeatherApiConsumerConfiguration
    {
        public const string Section = "WeatherApiConsumerConfiguration";
        public string ApiId { get; set; }
        public string ApiUrl { get; set; }
        public int Throttle { get; set; }
        public WeatherDataMetricSystem MetricSystem { get; set; }
    }
}

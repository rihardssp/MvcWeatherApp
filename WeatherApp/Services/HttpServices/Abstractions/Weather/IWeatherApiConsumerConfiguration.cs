using Services.Enums;

namespace Services.HttpServices.Abstractions.Weather
{
    public interface IWeatherApiConsumerConfiguration
    {
        string ApiId { get; }
        string ApiUrl { get; }
        int Throttle { get; }
        WeatherDataMetricSystem MetricSystem { get; }
    }
}

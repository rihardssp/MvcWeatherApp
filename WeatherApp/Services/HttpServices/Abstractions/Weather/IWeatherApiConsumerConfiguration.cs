using System;

namespace Services.HttpServices.Abstractions.Weather
{
    public interface IWeatherApiConsumerConfiguration
    {
        string ApiId { get; }
        string ApiUrl { get; }
    }
}

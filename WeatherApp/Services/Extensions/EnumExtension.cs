using OpenWeatherMap;
using Services.Enums;
using System;

namespace Services.Extensions
{
    public static class EnumExtension
    {
        public static MetricSystem ToOpenWeatherEnum(this WeatherDataMetricSystem value) {
            var valueString = value.ToString();
            return Enum.Parse<MetricSystem>(valueString);
        }
    }
}

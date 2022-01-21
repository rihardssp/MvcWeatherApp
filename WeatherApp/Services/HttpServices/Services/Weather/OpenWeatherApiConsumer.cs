using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenWeatherMap;
using Services.Extensions;
using Services.HttpServices.Abstractions.Weather;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.HttpServices.Services.Weather
{
    /// <summary>
    /// Consumes openweathermap.org/current
    /// Didn't find any service that was:
    /// 1) Free,
    /// 2) Provided updates less than 1h apart
    /// 3) Allowed multiple cities to be queried at once
    /// 
    /// Hence the implementation is a bit awkward
    /// 
    /// </summary>
    public class OpenWeatherApiConsumer : IWeatherApiConsumer
    {
        private IWeatherApiConsumerConfiguration _configuration;
        private ILogger<OpenWeatherApiConsumer> _logger;
        public OpenWeatherApiConsumer(IOptions<WeatherApiConsumerConfiguration> configuration, ILogger<OpenWeatherApiConsumer> logger)
        {
            _configuration = configuration.Value;
            _logger = logger;
        }

        public async Task<IList<WeatherRecord>> SendRequest(params int[] locationIds)
        {
            try
            {
                var client = new OpenWeatherMapClient(_configuration.ApiId);
                var result = new List<WeatherRecord>();

                foreach (var id in locationIds)
                {
                    try
                    {
                        var content = await client.CurrentWeather.GetByCityId(id, _configuration.MetricSystem.ToOpenWeatherEnum(), OpenWeatherMapLanguage.EN);
                        result.Add(new WeatherRecord
                        {
                            Temperature = content.Temperature.Value,
                            Date = content.LastUpdate.Value,
                            WindSpeed = content.Wind.Speed.Value,
                            LocationApiId = content.City.Id,
                            Cloudiness = content.Clouds.Value
                        });
                    } catch(Exception e)
                    {
                        _logger.LogError($"Failed to acquire data for '{id}'", e);
                    }

                    // Some throttle to avoid any issues with too many requests
                    await Task.Delay(_configuration.Throttle);
                }

                return result;
            } catch(Exception e)
            {
                _logger.LogError($"{nameof(OpenWeatherApiConsumer)} failed to obtain data for records {locationIds}", e);
                throw;
            }
        }
    }
}

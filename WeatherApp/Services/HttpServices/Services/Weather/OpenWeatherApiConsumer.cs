using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenWeatherMap;
using Services.HttpServices.Abstractions.Weather;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.HttpServices.Services.Weather
{
    // TODO: Important - check which date is being used here. Should convert to local.
    public class OpenWeatherApiConsumer : IWeatherApiConsumer
    {
        private IWeatherApiConsumerConfiguration _configuration;
        private ILogger<OpenWeatherApiConsumer> _logger;
        public OpenWeatherApiConsumer(IOptions<WeatherApiConsumerConfiguration> configuration, ILogger<OpenWeatherApiConsumer> logger)
        {
            _configuration = configuration.Value;
            _logger = logger;
        }

        /// <summary>
        /// TODO: Add cancellation token
        /// </summary>
        /// <param name="locationIds"></param>
        /// <returns></returns>
        public async Task<IList<WeatherRecord>> GetRequest(params int[] locationIds)
        {
            try
            {
                var client = new OpenWeatherMapClient(_configuration.ApiId);
                var result = new List<WeatherRecord>();

                foreach (var id in locationIds)
                {
                    var content = await client.CurrentWeather.GetByCityId(id, MetricSystem.Metric, OpenWeatherMapLanguage.EN);
                    result.Add(new WeatherRecord
                    {
                        Temperature = content.Temperature.Value,
                        Date = content.LastUpdate.Value,
                        WindSpeed = content.Wind.Speed.Value,
                        LocationApiId = content.City.Id
                    });
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

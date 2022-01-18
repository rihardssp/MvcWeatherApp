using Microsoft.Extensions.Logging;
using Services.Extensions;
using Services.HttpServices.Abstractions;
using Services.HttpServices.Abstractions.Weather;
using Services.HttpServices.GeneratedResponseTypes.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenWeatherMap;
using Microsoft.Extensions.Options;

namespace Services.HttpServices.Services.Weather
{
    /// <summary>
    /// An attempt at using the bulk operation so there wouldn't be a need for so many requests.
    /// Unfortunately bulk API only updates once per hour, so it doesn't work as expected for newest entries.
    /// </summary>
    public class OpenWeatherApiConsumerBulk : IWeatherApiConsumer
    {
        private static readonly HttpClient _client = new HttpClient();
        private IWeatherApiConsumerConfiguration _configuration;
        private ILogger<OpenWeatherApiConsumer> _logger;
        public OpenWeatherApiConsumerBulk(IOptions<IWeatherApiConsumerConfiguration> configuration, ILogger<OpenWeatherApiConsumer> logger)
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
            var urlBuilder = new UriBuilder(_configuration.ApiUrl);
            var query = new Dictionary<string, string>
            {
                { "appid", _configuration.ApiId },
                { "id", string.Join(",", locationIds) },
                { "units", "metric" }
            };

            urlBuilder.Query = query.ToUrlQuery();
            var result = await _client.GetAsync(urlBuilder.ToString());

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var myDeserializedClass = JsonSerializer.Deserialize<Root>(content);
                var recordList = myDeserializedClass.List.Select(e => WeatherRecord.MapRecord(e)).ToList();

                if (recordList.Count() != locationIds.Length)
                {
                    _logger.LogWarning($"Records returned count doesn't match record requested count!");
                    _logger.LogWarning($"Requested: {string.Join(",", locationIds)}");
                    _logger.LogWarning($"Received: {string.Join(",", recordList.Select(e => e.LocationApiId))}");
                }

                return recordList;
            }

            query.Remove("appid");
            urlBuilder.Query = query.ToUrlQuery();
            throw new HttpServiceException(result.StatusCode, $"Request failed: {result.ReasonPhrase}. Url (no appId): {urlBuilder}.");
        }
    }
}

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

namespace Services.HttpServices.Services.Weather
{
    /// <summary>
    /// Bulk operation of the same API OpenWeatherApiConsumer is using.
    /// Disadvantages: 
    /// 1) Suspect the API is being discontinued (documentation for this was hidden) 
    /// 2) Generated classes manually, no swagger exists either and no support from nuget
    /// 3) Updates HOURLY! No point in making multiple calls per hour here.
    /// </summary>
    public class OpenWeatherApiConsumerBulk : IWeatherApiConsumer
    {
        private static readonly HttpClient _client = new HttpClient();
        private IWeatherApiConsumerConfiguration _configuration;
        private ILogger<OpenWeatherApiConsumer> _logger;
        public OpenWeatherApiConsumerBulk(IOptions<WeatherApiConsumerConfiguration> configuration, ILogger<OpenWeatherApiConsumer> logger)
        {
            _configuration = configuration.Value;
            _logger = logger;
        }

        public async Task<IList<WeatherRecord>> SendRequest(params int[] locationIds)
        {
            var urlBuilder = new UriBuilder(_configuration.ApiUrl);
            var query = new Dictionary<string, string>
            {
                { "appid", _configuration.ApiId },
                { "id", string.Join(",", locationIds) },
                { "units", _configuration.MetricSystem.ToString().ToLower() }
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

using DataAccessLayer.Enums;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using WebMvc.Configuration;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    /// <summary>
    /// Displays property value as a line chart for the last few hours of data given location and property type
    /// </summary>
    [Route("[controller]")]
    public class WeatherTrendController : Controller
    {
        private readonly IWeatherRepositoryWrapper _repository;
        private readonly IDateTimeConfiguration _dateTimeUtil;
        private readonly IWeatherConfiguration _configuration;
        public WeatherTrendController(IWeatherRepositoryWrapper repository, 
            IOptions<DateTimeConfiguration> dateTimeUtil, 
            IOptions<WeatherConfiguration> configuration)
        {
            _dateTimeUtil = dateTimeUtil.Value;
            _configuration = configuration.Value;
            _repository = repository;
        }

        /// <summary>
        /// Creates a chart with a given description and data queried by using location id and typeid
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private IActionResult GetTrend(int locationId, AttributeType type, string description)
        {
            var location = _repository.Location.FindAll().FirstOrDefault(e => e.Id == locationId);

            if (location == null)
            {
                return NotFound();
            }

            var dateFrom = DateTime.UtcNow.AddHours(-_configuration.TrendLengthHours);
            var dateTo = DateTime.UtcNow;
            var dateFormat = dateFrom.Day == dateTo.Day ? _dateTimeUtil.TimeFormat : _dateTimeUtil.ShortenedDayTimeFormat;

            // Main data query
            var data = _repository.WeatherAttribute.FindAll()
                .Where(e => e.LocationId == location.Id && e.Date >= dateFrom && e.TypeId == (int)type)
                .OrderBy(e => e.Date)
                .Select(e => new ChartEntryViewModel
                { 
                    Value = e.ValueDouble,
                    Label = e.Date.ToString(dateFormat)
                })
                .ToArray();

            // Didn't know what else to do with this data
            var cloudiness = _repository.WeatherAttribute.FindAll()
                .Where(e => e.LocationId == locationId && e.TypeId == (int)AttributeType.CloudinessPercentage)
                .FirstOrDefault();

            var cloudinessText = cloudiness == null ? "No cloudiness data available"
                : $"On {cloudiness.Date.ToString(_dateTimeUtil.FullFormat)} UTC cloudiness in " +
                $"{location.City} was {(int)Math.Truncate(cloudiness.ValueDouble)}%";

            return View("Trend", new TrendViewModel
            {
                Data = data,
                ChartDescription = description,
                DateFrom = dateFrom,
                DateTo = dateTo,
                DateFormat = dateFormat,
                Description = $"{location.City}, {location.Country}",
                Cloudiness = cloudinessText
            });
        }

        /// <summary>
        /// Defines a route to temperature property type chart
        /// </summary>
        /// <param name="id">locationId</param>
        /// <returns></returns>
        [Route("[action]/{id}")]
        public IActionResult Temperature([FromRoute] int id)
        {
            return GetTrend(id, AttributeType.TemperatureCelsius, "Temperature (Celsius)");
        }

        /// <summary>
        /// Defines a route to wind property type chart
        /// </summary>
        /// <param name="id">locationId</param>
        /// <returns></returns>
        [Route("[action]/{id}")]
        public IActionResult WindSpeed([FromRoute] int id)
        {
            return GetTrend(id, AttributeType.WindSpeedMs, "Wind speed (m/sec)");
        }
    }
}

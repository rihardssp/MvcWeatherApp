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

        private IActionResult GetTrend(int id, AttributeType type, string description)
        {
            var location = _repository.Location.FindAll().FirstOrDefault(e => e.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            var dateFrom = DateTime.UtcNow.AddHours(-_configuration.TrendLengthHours);
            var dateTo = DateTime.UtcNow;
            var dateFormat = dateFrom.Day == dateTo.Day ? _dateTimeUtil.TimeFormat : _dateTimeUtil.ShortenedDayTimeFormat;

            var data = _repository.WeatherAttribute.FindAll()
                .Where(e => e.LocationId == location.Id && e.Date >= dateFrom && e.TypeId == (int)type)
                .OrderBy(e => e.Date)
                .Select(e => new ChartEntryViewModel
                { 
                    Value = e.ValueDouble,
                    Label = e.Date.ToString(dateFormat)
                })
                .ToArray();

            var cloudiness = _repository.WeatherAttribute.FindAll()
                .Where(e => e.LocationId == id && e.TypeId == (int)AttributeType.CloudinessPercentage)
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

        [Route("[action]/{id}")]
        public IActionResult Temperature([FromRoute] int id)
        {
            return GetTrend(id, AttributeType.TemperatureCelsius, "Temperature (Celsius)");
        }

        [Route("[action]/{id}")]
        public IActionResult WindSpeed([FromRoute] int id)
        {
            return GetTrend(id, AttributeType.WindSpeedMs, "Wind speed (m/sec)");
        }
    }
}

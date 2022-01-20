using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    [Route("[controller]")]
    public class WeatherTrendController : Controller
    {
        private readonly IWeatherRepositoryWrapper _repository;

        public WeatherTrendController(IWeatherRepositoryWrapper repository)
        {
            _repository = repository;
        }

        private IActionResult GetTrend(int id, AttributeType type, string description)
        {
            var location = _repository.Location.FindAll().FirstOrDefault(e => e.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            // TODO: Move this to configuration. 
            var dateFrom = DateTime.Now.AddHours(-2);
            var dateTo = DateTime.Now;
            var dateFormat = dateFrom.Day == dateTo.Day ? "HH:mm" : "ddd HH:mm";

            var data = _repository.WeatherAttribute.FindAll()
                .Where(e => e.LocationId == location.Id && e.Date >= dateFrom && e.TypeId == (int)type)
                .OrderBy(e => e.Date)
                .Select(e => new ChartEntryViewModel
                { 
                    Value = e.ValueDouble,
                    Label = e.Date.ToString(dateFormat)
                })
                .ToArray();

            return View("Trend", new TrendViewModel
            {
                Data = data,
                ChartDescription = description,
                DateFrom = dateFrom,
                DateTo = dateTo,
                DateFormat = dateFormat,
                Description = $"{location.City}, {location.Country}"
            });
        }

        [Route("[action]/{id}")]
        public IActionResult Temperature([FromRoute] int id)
        {
            // TODO: prehaps move the descriptions to a resource file?
            return GetTrend(id, AttributeType.TemperatureCelsius, "Temperature (Celsius)");
        }

        [Route("[action]/{id}")]
        public IActionResult WindSpeed([FromRoute] int id)
        {
            return GetTrend(id, AttributeType.WindSpeedMs, "Wind speed (m/sec)");
        }
    }
}

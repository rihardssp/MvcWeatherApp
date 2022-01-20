using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
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

        private IActionResult GetTrend(int id, Func<WeatherEntryModel, double> valueSelector, string description)
        {
            var location = _repository.Location.FindAll().FirstOrDefault(e => e.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            var dateFrom = DateTime.Now.AddHours(-2);
            var dateTo = DateTime.Now;

            var data = _repository.WeatherEntry.FindAll()
                .Where(e => e.LocationId == location.Id && e.Date >= dateFrom)
                .Select(e => new TrendItemEntry { Value = valueSelector(e), Date = e.Date })
                .OrderBy(e => e.Date)
                .ToList();

            var dateFormat = dateFrom.Day == dateTo.Day ? "HH:mm" : "ddd HH:mm";

            return View("Trend", new TrendViewModel { Entries = data, ChartDescription = description, 
                DateFrom = dateFrom, 
                DateTo = dateTo,
                DateFormat = dateFormat
            });
        }

        [Route("[action]/{id}")]
        public IActionResult Temperature([FromRoute] int id)
        {
            // TODO: prehaps move the descriptions to a resource file?
            return GetTrend(id, e => e.Temperature, "Temperature (Celsius)");
        }

        [Route("[action]/{id}")]
        public IActionResult WindSpeed([FromRoute] int id)
        {
            return GetTrend(id, e => e.WindSpeed, "Wind speed (m/sec)");
        }
    }
}

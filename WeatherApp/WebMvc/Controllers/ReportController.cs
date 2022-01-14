using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMvc.Data;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class ReportController : Controller
    {
        private readonly WeatherContext _context;
        public ReportController(WeatherContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var locationGroups = _context.WeatherEntryModel.GroupBy(e => e.Location).Select(e => e.Key);

            var query = from weather in _context.WeatherEntryModel.AsNoTracking()
                group weather by weather.LocationId into locationGroup
                select new
                {
                    locationGroup.Key,
                    temp = locationGroup.Min(e => e.Temperature),
                    wind = locationGroup.Max(e => e.WindSpeed),
                } into groupedValues
                join location in _context.LocationModel.AsNoTracking()
                    on groupedValues.Key equals location.Id
                select new WeatherExtremesViewModel
                {
                    Location = location,
                    MinimumTemperature = groupedValues.temp,
                    MaximumWindSpeed = groupedValues.wind,
                };

            var extremesData = query.ToList();
            return View(extremesData);
        }
    }
}

using DataAccessLayer.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
                    MinimumTemperature = locationGroup.Min(e => e.Temperature),
                    MaximumWindSpeed = locationGroup.Max(e => e.WindSpeed),
                    LastUpdate = locationGroup.Max(e => e.Date)
                } into groupedValues
                join location in _context.LocationModel.AsNoTracking()
                    on groupedValues.Key equals location.Id
                select new WeatherExtremesViewModel
                {
                    Location = location,
                    MinimumTemperature = groupedValues.MinimumTemperature,
                    MaximumWindSpeed = groupedValues.MaximumWindSpeed,
                    LastUpdate = groupedValues.LastUpdate
                };

            var extremesData = query.ToList();
            return View(extremesData);
        }
    }
}

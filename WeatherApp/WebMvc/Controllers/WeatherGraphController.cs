using DataAccessLayer.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class WeatherGraphController : Controller
    {
        private readonly WeatherContext _context;
        public WeatherGraphController(WeatherContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
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
                        select new WeatherGraphItemViewModel
                        {
                            Location = location,
                            MinimumTemperature = groupedValues.MinimumTemperature,
                            MaximumWindSpeed = groupedValues.MaximumWindSpeed,
                            LastUpdate = groupedValues.LastUpdate
                        };

            var extremesData = query.OrderBy(e => e.Location.City).ToList();
            return View(extremesData);
        }
    }
}

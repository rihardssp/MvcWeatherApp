using DataAccessLayer.Contexts;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebMvc.Code.Enums;
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
            var query = from weather in _context.WeatherAttributeModel.AsNoTracking()
                         group weather by new { weather.LocationId, weather.TypeId } into locationGroup
                         select new
                         {
                             locationGroup.Key.LocationId,
                             locationGroup.Key.TypeId,
                             minimumValue = locationGroup.Min(e => e.ValueDouble),
                             maximumValue = locationGroup.Max(e => e.ValueDouble),
                             LastUpdate = locationGroup.Max(e => e.Date)
                         } into groupedValues
                         join location in _context.LocationModel.AsNoTracking()
                             on groupedValues.LocationId equals location.Id
                         select new
                         {
                             Location = location,
                             Minimum = groupedValues.minimumValue,
                             Maximum = groupedValues.maximumValue,
                             TypeId = groupedValues.TypeId,
                             LastUpdate = groupedValues.LastUpdate
                         };

            var data = query.OrderBy(e => e.Location.City);
            var data2 = data.ToList();

            var dataTemp = data2.Where(e => e.TypeId == (int)AttributeType.TemperatureCelsius);
            var dataWind = data2.Where(e => e.TypeId == (int)AttributeType.WindSpeedMs);

            var temperatureList = new WeatherGraphItemViewModel
            {
                ChartModel = new ChartViewModel
                {
                    Id = "temperature",
                    CssClass = "chartutil-pointer",
                    Description = "Minimum temperature (celsius)",
                    ChartType = ChartType.bar,
                    Data = dataTemp
                        .Select(e => new ChartEntryViewModel { 
                            Label = e.Location.City, 
                            Value = e.Minimum, 
                            Tooltip =  new string[] { $"Last update: {e.LastUpdate.ToString("dd/MM/yyyy HH:mm")}", $"Country: {e.Location.Country}" },
                            ActionId = e.Location.Id
                        })
                        .ToArray()
                },
                ActionUrl = Url.Action("Temperature", nameof(WeatherTrendController).Replace("Controller", string.Empty))
            };

            var windList = new WeatherGraphItemViewModel {
                ChartModel = new ChartViewModel
                {
                    Id = "wind",
                    CssClass = "chartutil-pointer",
                    Description = "Maximum wind speed (m/s)",
                    ChartType = ChartType.bar,
                    Data = dataTemp
                        .Select(e => new ChartEntryViewModel
                        {
                            Label = e.Location.City,
                            Value = e.Maximum,
                            Tooltip = new string[] { $"Last update: {e.LastUpdate.ToString("dd/MM/yyyy HH:mm")}", $"Country: {e.Location.Country}" },
                            ActionId = e.Location.Id
                        })
                        .ToArray()
                },
                ActionUrl = Url.Action("WindSpeed", nameof(WeatherTrendController).Replace("Controller", string.Empty)),
            };

            return View(new List<WeatherGraphItemViewModel> { temperatureList, windList });
        }
    }
}

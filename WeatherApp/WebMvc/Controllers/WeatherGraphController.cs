using DataAccessLayer.Contexts;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using WebMvc.Code.Enums;
using WebMvc.Configuration;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class WeatherGraphController : Controller
    {
        private readonly WeatherContext _context;
        private readonly IDateTimeConfiguration _dateTimeUtil;
        public WeatherGraphController(WeatherContext context, IOptions<DateTimeConfiguration> dateTimeUtil)
        {
            _dateTimeUtil = dateTimeUtil.Value;
            _context = context;
        }

        public IActionResult Index()
        {
            var allowedTypes = new int[] { (int)AttributeType.TemperatureCelsius, (int)AttributeType.WindSpeedMs };

            var query = from weather in _context.WeatherAttributeModel.AsNoTracking()
                        where allowedTypes.Contains(weather.TypeId)
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
                        } into finalData
                        orderby finalData.Location.Country
                        orderby finalData.Location.City
                        select finalData;

            // call DB only once
            var data = query.ToList();
            var temperatureData = data.Where(e => e.TypeId == (int)AttributeType.TemperatureCelsius)
                .Select(e => new ChartEntryViewModel
                {
                    Label = e.Location.City,
                    Value = e.Minimum,
                    Tooltip = new string[] { $"Last update (UTC): {e.LastUpdate.ToString(_dateTimeUtil.FullFormat)}", $"Country: {e.Location.Country}" },
                    ActionId = e.Location.Id
                })
                .ToArray();

            var windData = data.Where(e => e.TypeId == (int)AttributeType.WindSpeedMs)
                .Select(e => new ChartEntryViewModel
                {
                    Label = e.Location.City,
                    Value = e.Maximum,
                    Tooltip = new string[] { $"Last update (UTC): {e.LastUpdate.ToString(_dateTimeUtil.FullFormat)}", $"Country: {e.Location.Country}" },
                    ActionId = e.Location.Id
                })
                .ToArray();

            return View(new List<ChartWithActionViewModel> {
                CreateChartViewModel("temperature", "Minimum temperature (celsius)", temperatureData, nameof(WeatherTrendController.Temperature)),
                CreateChartViewModel("wind", "Maximum wind speed (m/s)", windData, nameof(WeatherTrendController.WindSpeed))
            });
        }

        private ChartWithActionViewModel CreateChartViewModel(string id, string description, ChartEntryViewModel[] data, string controllerMethodName)
        {
            return new ChartWithActionViewModel
            {
                ChartModel = new ChartViewModel
                {
                    Id = id,
                    CssClass = "chartutil-pointer",
                    Description = description,
                    ChartType = ChartType.bar,
                    Data = data
                },
                ActionUrl = Url.Action(controllerMethodName, nameof(WeatherTrendController).Replace("Controller", string.Empty)),
            };
        }
    }
}

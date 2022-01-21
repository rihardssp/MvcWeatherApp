using DataAccessLayer.Contexts;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WebMvc.Code.Enums;
using WebMvc.Code.HelperClasses;
using WebMvc.Configuration;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    /// <summary>
    /// For every city displays max wind, and min temperature
    /// </summary>
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
            // Per each location get the two attribute values' extremes and last updates
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
                        select new TrendValues
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

            // Select data for charts - for each type
            return View(new List<ChartWithActionViewModel> {
                CreateChartViewModel("temperature", "Minimum temperature (celsius)", data, 
                    AttributeType.TemperatureCelsius, nameof(WeatherTrendController.Temperature)),
                CreateChartViewModel("wind", "Maximum wind speed (m/s)", data, 
                    AttributeType.WindSpeedMs, nameof(WeatherTrendController.WindSpeed))
            });
        }

        /// <summary>
        /// Transforms TrendValues into chart data.
        /// </summary>
        /// <param name="id">ChartId</param>
        /// <param name="description">Chart description</param>
        /// <param name="data"></param>
        /// <param name="type">Attribute type that will be displayed</param>
        /// <param name="controllerMethodName">Name of controller method</param>
        /// <returns></returns>
        private ChartWithActionViewModel CreateChartViewModel(string id, string description, List<TrendValues> data, 
            AttributeType type, string controllerMethodName)
        {
            var chartData = data.Where(e => e.TypeId == (int)type)
                .Select(e => new ChartEntryViewModel
                {
                    Label = e.Location.City,
                    Value = type == AttributeType.WindSpeedMs ? e.Maximum : e.Minimum,
                    Tooltip = new string[]
                    {
                        $"Last update (UTC): {e.LastUpdate.ToString(_dateTimeUtil.FullFormat)}",
                        $"Country: {e.Location.Country}"
                    },
                    ActionId = e.Location.Id
                })
                .ToArray();

            return new ChartWithActionViewModel
            {
                ChartModel = new ChartViewModel
                {
                    Id = id,
                    CssClass = "chartutil-pointer",
                    Description = description,
                    ChartType = ChartType.bar,
                    Data = chartData
                },
                ActionUrl = Url.Action(controllerMethodName, "WeatherTrend"),
            };
        }
    }
}

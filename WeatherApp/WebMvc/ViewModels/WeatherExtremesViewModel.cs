using DataAccessLayer.Models;
using System;

namespace WebMvc.ViewModels
{
    public class WeatherExtremesViewModel
    {
        public LocationModel Location { get; set; }
        public DateTime LastUpdate { get; set; }
        public double MinimumTemperature { get; set; }
        public double MaximumWindSpeed { get; set; }
    }
}

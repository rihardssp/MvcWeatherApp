using DataAccessLayer.Models;
using System;

namespace WebMvc.ViewModels
{
    public class WeatherExtremesViewModel
    {
        public LocationModel Location { get; set; }
        public DateTime LastUpdate { get; set; }
        public int MinimumTemperature { get; set; }
        public int MaximumWindSpeed { get; set; }
    }
}

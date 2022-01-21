using DataAccessLayer.Models;
using System;

namespace WebMvc.Code.HelperClasses
{
    internal class TrendValues
    {
        public LocationModel Location { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public int TypeId { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
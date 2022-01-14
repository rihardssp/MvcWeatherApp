using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int ApiId { get; set; }
        public virtual IEnumerable<WeatherEntryModel> WeatherEntries { get; set; }
    }
}

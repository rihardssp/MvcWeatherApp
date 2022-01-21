using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    /// <summary>
    /// Defines 'location' with city name / country name and any API references.
    /// Didn't bother with normalisation as the scope of this app is tiny.
    /// </summary>
    public class LocationModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int ApiId { get; set; }
    }
}

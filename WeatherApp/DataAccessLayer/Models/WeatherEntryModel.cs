using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class WeatherEntryModel
    {
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Location")]
        public int LocationId { get; set; }

        [Required]
        public LocationModel Location { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
    }
}

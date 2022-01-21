using DataAccessLayer.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    /// <summary>
    /// To avoid any difficult scenarios using pivot pattern decouple attributes from structure
    /// This'll allow adding independent services for measurements independent of the frontend, and enabling them only as necessary later.
    /// </summary>
    public class WeatherAttributeModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public LocationModel Location { get; set; }

        [Required]
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public WeatherAttributeTypeModel Type { get; set; }
        public double ValueDouble { get; set; }
    }
}

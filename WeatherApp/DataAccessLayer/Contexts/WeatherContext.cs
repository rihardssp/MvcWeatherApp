using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Contexts
{
    /// <summary>
    /// Context for the Weather domain
    /// </summary>
    public class WeatherContext : DbContext
    {
        public WeatherContext() { }
        public WeatherContext (DbContextOptions<WeatherContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherEntryModel> WeatherEntryModel { get; set; }

        public DbSet<LocationModel> LocationModel { get; set; }

        
    }
}

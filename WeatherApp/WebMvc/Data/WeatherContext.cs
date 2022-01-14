using CoreWeatherApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebMvc.Data
{
    public class WeatherContext : DbContext
    {
        public WeatherContext (DbContextOptions<WeatherContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherEntryModel> WeatherEntryModel { get; set; }

        public DbSet<CoreWeatherApp.Models.LocationModel> LocationModel { get; set; }
    }
}

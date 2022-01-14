using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Contexts
{
    public class WeatherContext : DbContext
    {
        public WeatherContext() { }
        public WeatherContext (DbContextOptions<WeatherContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherEntryModel> WeatherEntryModel { get; set; }

        public DbSet<LocationModel> LocationModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=localhost;user=sa;password=Ex0Mplew@!3kew;database=MeteorologicalDb;");
        }
    }
}

using DataAccessLayer.MigrationExtensions;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

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
        public DbSet<WeatherAttributeModel> WeatherAttributeModel { get; set; }
        public DbSet<WeatherAttributeTypeModel> WeatherAttributeTypeModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
                .ReplaceService<IMigrationsSqlGenerator, SqlServerMigrationGeneratorExtension>();
    }
}

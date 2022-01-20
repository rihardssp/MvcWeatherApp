using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using DataAccessLayer.RepositoryAbstractions;

namespace DataAccessLayer.Repositories.Weather
{
    public class WeatherEntryRepository : RepositoryBase<WeatherEntryModel>, IWeatherEntryRepository
    {
        public WeatherEntryRepository(WeatherContext dbContext) : base(dbContext)
        {
        }
    }
}

using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using DataAccessLayer.RepositoryAbstractions;

namespace DataAccessLayer.Repositories.Weather
{
    public class WeatherAttributeRepository : RepositoryBase<WeatherAttributeModel>, IWeatherAttributeRepository
    {
        public WeatherAttributeRepository(WeatherContext dbContext) : base(dbContext)
        {
        }
    }
}

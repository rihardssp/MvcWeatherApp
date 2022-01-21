using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using DataAccessLayer.RepositoryAbstractions;

namespace DataAccessLayer.Repositories.Weather
{
    public class WeatherAttributeTypeRepository : RepositoryBase<WeatherAttributeTypeModel>, IWeatherAttributeTypeRepository
    {
        public WeatherAttributeTypeRepository(WeatherContext dbContext) : base(dbContext)
        {
        }
    }
}

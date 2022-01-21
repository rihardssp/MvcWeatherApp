using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Weather;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Holds all repositories to make them accessible by injection easily.
    /// </summary>
    public class WeatherRepositoryWrapper : IWeatherRepositoryWrapper
    {
        private WeatherContext _dbContext;
        private ILocationRepository _location;
        private IWeatherAttributeRepository _weatherAttribute;
        private IWeatherAttributeTypeRepository _weatherAttributeType;

        public ILocationRepository Location
        {
            get
            {
                if (_location == null)
                {
                    _location = new LocationRepository(_dbContext);
                }
                return _location;
            }
        }

        public IWeatherAttributeRepository WeatherAttribute
        {
            get
            {
                if (_weatherAttribute == null)
                {
                    _weatherAttribute = new WeatherAttributeRepository(_dbContext);
                }
                return _weatherAttribute;
            }
        }

        public IWeatherAttributeTypeRepository WeatherAttributeType
        {
            get
            {
                if (_weatherAttributeType == null)
                {
                    _weatherAttributeType = new WeatherAttributeTypeRepository(_dbContext);
                }
                return _weatherAttributeType;
            }
        }

        public WeatherRepositoryWrapper(WeatherContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}

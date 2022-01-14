using DataAccessLayer.Contexts;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private WeatherContext _dbContext;
        private ILocationRepository _location;
        private IWeatherEntryRepository _weatherEntry;
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
        public IWeatherEntryRepository WeatherEntry
        {
            get
            {
                if (_weatherEntry == null)
                {
                    _weatherEntry = new WeatherEntryRepository(_dbContext);
                }
                return _weatherEntry;
            }
        }

        public RepositoryWrapper(WeatherContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}

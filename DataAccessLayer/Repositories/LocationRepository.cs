using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using DataAccessLayer.RepositoryAbstractions;

namespace DataAccessLayer.Repositories
{
    public class LocationRepository : RepositoryBase<LocationModel>, ILocationRepository
    {
        public LocationRepository(WeatherContext dbContext) : base(dbContext)
        {
        }
    }
}

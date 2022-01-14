using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IRepositoryWrapper
    {
        ILocationRepository Location { get; }
        IWeatherEntryRepository WeatherEntry { get; }

        Task<int> SaveChangesAsync();
    }
}

using DataAccessLayer.Repositories.Weather;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// The main repository for the Weather domain
    /// </summary>
    public interface IWeatherRepositoryWrapper
    {
        ILocationRepository Location { get; }
        IWeatherEntryRepository WeatherEntry { get; }
        Task<int> SaveChangesAsync();
    }
}

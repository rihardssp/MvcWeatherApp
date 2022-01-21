using DataAccessLayer.Repositories.Weather;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    /// <summary>
    /// Holds all repositories to make them accessible by injection easily.
    /// </summary>
    public interface IWeatherRepositoryWrapper
    {
        ILocationRepository Location { get; }
        IWeatherAttributeRepository WeatherAttribute { get; }
        IWeatherAttributeTypeRepository WeatherAttributeType { get; }
        Task<int> SaveChangesAsync();
    }
}

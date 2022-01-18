using DataAccessLayer.Models;
using DataAccessLayer.RepositoryAbstractions;

namespace DataAccessLayer.Repositories.Weather
{
    public interface IWeatherEntryRepository : IRepositoryBase<WeatherEntryModel>
    {
    }
}

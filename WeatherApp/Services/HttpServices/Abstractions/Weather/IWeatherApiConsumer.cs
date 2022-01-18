using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.HttpServices.Abstractions.Weather
{
    public interface IWeatherApiConsumer
    {
        Task<IList<WeatherRecord>> GetRequest(params int[] locationIds);
    }
}
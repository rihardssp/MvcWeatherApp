using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.HttpServices.Abstractions.Weather
{
    /// <summary>
    /// Consumes weather APIs to return weather related data per city
    /// </summary>
    public interface IWeatherApiConsumer
    {
        /// <summary>
        /// Send request to weather api to receive weather data for given locations
        /// Note: Might contain several api calls
        /// </summary>
        /// <param name="locationIds"></param>
        /// <returns></returns>
        Task<IList<WeatherRecord>> SendRequest(params int[] locationIds);
    }
}
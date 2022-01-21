using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Registers data layer repositories
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection UseRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IWeatherRepositoryWrapper, WeatherRepositoryWrapper>();
            return serviceCollection;
        }
    }
}

using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IWeatherRepositoryWrapper, WeatherRepositoryWrapper>();
            return serviceCollection;
        }
    }
}

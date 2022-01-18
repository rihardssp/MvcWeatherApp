using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.HttpServices.Abstractions.Weather;
using Services.HttpServices.Services.Weather;

namespace DataAccessLayer.Extensions
{
    /// <summary>
    /// Make the registration of consumer way more simple - no need to register the configuration separately
    /// </summary>
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseOpenWeatherConsumer(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<WeatherApiConsumerConfiguration>(configuration.GetSection(WeatherApiConsumerConfiguration.Section));
            serviceCollection.AddSingleton<IWeatherApiConsumer>((service) => 
                new OpenWeatherApiConsumer(service.GetRequiredService<IOptions<WeatherApiConsumerConfiguration>>(), service.GetRequiredService<ILogger<OpenWeatherApiConsumer>>()));
            return serviceCollection;
        }
    }
}

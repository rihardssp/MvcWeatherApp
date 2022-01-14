using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace WebMvc
{
    public static class ServiceCollectionExtension
    {
        public static void UseRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}

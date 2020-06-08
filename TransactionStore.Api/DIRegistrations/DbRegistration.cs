using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TransactionStore.Api.DIRegistrations
{
    public static class DbRegistration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection;
        }
    }
}
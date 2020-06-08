using Microsoft.Extensions.DependencyInjection;

namespace TransactionStore.Api.DIRegistrations
{
    public static class ServicesRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionStore.Data;

namespace TransactionStore.Api.DIRegistrations
{
    public static class DbRegistration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PaymentTransactionsDb");
            serviceCollection
                .AddDbContext<AppDbContext>(options => { options.UseNpgsql(connectionString); })
                .AddHealthChecks()
                .AddNpgSql(connectionString, tags: new[] {"db", "sql", "postgres", "npgsql"});

            return serviceCollection;
        }
    }
}
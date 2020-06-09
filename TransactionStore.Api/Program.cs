using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransactionStore.Data;

namespace TransactionStore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            PerformDatabaseMigrations(host);

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(w => w.AddConsole())
                .UseStartup<Startup>();
        }

        private static void PerformDatabaseMigrations(IWebHost host)
        {
            using var serviceScope = host.Services.CreateScope();

            var environment = serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();

            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            if (!context.Database.IsInMemory() && environment.IsDevelopment()) context.Database.Migrate();
        }
    }
}
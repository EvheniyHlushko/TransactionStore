using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TransactionStore.Api.Tests.Mocks;
using TransactionStore.Client;
using TransactionStore.Client.Contracts;
using TransactionStore.Data;

namespace TransactionStore.Api.Tests.Helpers
{
    public class TransactionStoreWebFactory : WebApplicationFactory<TestStartup>
    {
        private readonly SqliteConnection _connection;

        public TransactionStoreWebFactory()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            TransactionsClient = base.Services.GetService<ITransactionsClient>();
            FileUploadClient = base.Services.GetService<ITransactionFileUploadClient>();
        }

        public AppDbContext Context { get; private set; }

        public ITransactionsClient TransactionsClient { get; }
        public ITransactionFileUploadClient FileUploadClient { get; }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .ConfigureAppConfiguration(ConfigureAppSettings)
                .UseStartup<TestStartup>()
                .ConfigureTestServices(services =>
                {
                    services.TryAddScoped<ITransactionsClient>(o => new TransactionsClient(Server.CreateClient()));
                    services.TryAddScoped<ITransactionFileUploadClient>(o => new TransactionFileUploadClient(Server.CreateClient()));

                    services.AddDbContext<AppDbContext, TestDbContext>(options => options.UseSqlite(_connection));

                    Context = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
                    Context.Database.EnsureCreated();
                });
        }

        private static void ConfigureAppSettings(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            var env = context.HostingEnvironment;
            builder.SetBasePath(Path.GetDirectoryName(new Uri(typeof(Startup).Assembly.CodeBase!).LocalPath));

            builder
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
                _connection?.Close();
                _connection?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
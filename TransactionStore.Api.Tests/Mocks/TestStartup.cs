using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using TransactionStore.Api.DIRegistrations;
using TransactionStore.Api.Extensions;
using TransactionStore.Api.Models;

namespace TransactionStore.Api.Tests.Mocks
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc(options => { options.EnableEndpointRouting = false; })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.UseCamelCasing(true);
                });

            mvcBuilder.PartManager.ApplicationParts.Clear();
            mvcBuilder.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));

            services.AddHttpClient();


            services.AddCors(t => t.AddDefaultPolicy(builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddMediatR(typeof(Startup).Assembly);

            services.RegisterServices();

            services.AddOptions()
                .Configure<FileRestrictionsSettings>(Configuration.GetSection("FileRestrictions"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseValidationExceptionHandler();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
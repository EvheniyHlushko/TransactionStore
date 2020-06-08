using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TransactionStore.Api.DIRegistrations;
using TransactionStore.Api.Extensions;
using TransactionStore.Api.Models;

namespace TransactionStore.Api
{
    public class Startup
    {
        private const string AppTitle = "AlphaFX Trade Settlement API";
        private static readonly Assembly StartupAssembly = typeof(Startup).Assembly;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDatabase(_configuration);

            services.AddMvc(options => { options.EnableEndpointRouting = false; })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.UseCamelCasing(true);
                });


            services.AddCors(t => t.AddDefaultPolicy(builder => builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSwaggerDocument(c =>
            {
                c.PostProcess = document =>
                {
                    document.Info.Title = AppTitle;
                    document.Info.Version = "v1";
                    document.Info.Description = $"Build version={StartupAssembly.GetName().Version}";
                };

                c.SerializerSettings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                    {
                        new StringEnumConverter()
                    },
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                c.SchemaGenerator = new OpenApiSchemaNSWagExampleGenerator(c.SchemaGenerator.Settings);
            });


            var version = new Dictionary<string, object> {{"version", StartupAssembly.GetName().Version!.ToString()}};
            services.AddHealthChecks().AddCheck("self",
                () => HealthCheckResult.Healthy(AppTitle,
                    new ReadOnlyDictionary<string, object>(version)));
            services.AddAutoMapper(StartupAssembly);
            services.RegisterServices();
            services.AddMediatR(StartupAssembly);

            services.AddOptions()
                .Configure<FileRestrictionsSettings>(_configuration.GetSection("FileRestrictions"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCors();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseValidationExceptionHandler();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
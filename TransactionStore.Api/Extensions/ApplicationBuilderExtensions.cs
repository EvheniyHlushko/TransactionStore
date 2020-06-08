using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransactionStore.Api.Infrastructure;

namespace TransactionStore.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler(errorApp =>
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (errorFeature.Error is ValidationException validationEx)
                    {
                        context.Response.ContentType = "application/problem+json";
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        var errors = validationEx.Errors
                            .ToDictionary(
                                validationError => validationError.PropertyName,
                                validationError => new[] {validationError.ErrorMessage}
                            );

                        var options = new JsonSerializerOptions {IgnoreNullValues = true};
                        await JsonSerializer.SerializeAsync(context.Response.Body, new ValidationProblemDetails(errors), options);
                    }
                })
            );
        }
    }
}
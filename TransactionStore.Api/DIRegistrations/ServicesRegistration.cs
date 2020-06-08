using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TransactionStore.Api.DomainEvents.Commands;
using TransactionStore.Api.DomainEvents.PipelineBehaviors;
using TransactionStore.Api.Infrastructure;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Api.Parsers;
using TransactionStore.Api.Parsers.Transaction;
using TransactionStore.Api.Validators;
using TransactionStore.Api.Validators.Transaction;

namespace TransactionStore.Api.DIRegistrations
{
    public static class ServicesRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IParser<PaymentTransaction>, TransactionXmlParser>();
            serviceCollection.AddScoped<IParser<PaymentTransaction>, TransactionCsvParser>();
            serviceCollection.AddScoped<IParserStrategy<PaymentTransaction>, ParserStrategy<PaymentTransaction>>();

            serviceCollection.AddScoped<IODataTools, ODataTools>();

            serviceCollection
                .AddScoped<ISequencedValidationService<FileParsingValidationContext<PaymentTransaction>>, TransactionValidator>();
            serviceCollection
                .AddScoped<IValidationService<FileParsingValidationContext<PaymentTransaction>>, TransactionPropertiesValidator>();

            serviceCollection
                .AddTransient<IPipelineBehavior<UploadTransactionsRequest, TransactionResponse>, TransactionFileValidator>();

            serviceCollection
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestExceptionHandler<,>));

            return serviceCollection;
        }
    }
}
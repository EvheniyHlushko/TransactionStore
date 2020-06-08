using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionStore.Api.Extensions;
using TransactionStore.Api.Infrastructure;

namespace TransactionStore.Api.DomainEvents.PipelineBehaviors
{
    public class RequestExceptionHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<RequestExceptionHandler<TRequest, TResponse>> _logger;

        public RequestExceptionHandler(ILogger<RequestExceptionHandler<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (ValidationException validationException)
            {
                _logger.LogError(validationException.Errors);
                throw;
            }
        }
    }
}
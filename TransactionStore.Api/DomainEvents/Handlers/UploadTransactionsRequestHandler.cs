using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TransactionStore.Api.DomainEvents.Commands;
using TransactionStore.Api.Infrastructure;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Api.Parsers;

namespace TransactionStore.Api.DomainEvents.Handlers
{
    public class UploadTransactionsRequestHandler : IRequestHandler<UploadTransactionsRequest, TransactionResponse>
    {
        private readonly IParserStrategy<PaymentTransaction> _parserStrategy;

        public UploadTransactionsRequestHandler(
            IParserStrategy<PaymentTransaction> parserStrategy)
        {
            _parserStrategy = parserStrategy;
        }

        public async Task<TransactionResponse> Handle(UploadTransactionsRequest request, CancellationToken cancellationToken)
        {
            var parsedResult = await _parserStrategy.ParseAsync(request.File, cancellationToken);

            if (parsedResult.ValidationResult.IsValid) return new TransactionResponse {Transactions = parsedResult.ParsedTransactions};

            throw new ValidationException(parsedResult.ValidationResult);
        }
    }
}
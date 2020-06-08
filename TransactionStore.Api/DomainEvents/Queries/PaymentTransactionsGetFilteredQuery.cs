using MediatR;
using TransactionStore.Api.Models.OData;
using TransactionStore.Api.Models.Transaction;

namespace TransactionStore.Api.DomainEvents.Queries
{
    public class PaymentTransactionsGetFilteredQuery : IRequest<ODataResult<PaymentTransaction>>
    {
        public ODataQuery ODataQuery { get; set; } = default!;
    }
}
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TransactionStore.Api.DomainEvents.Queries;
using TransactionStore.Api.Infrastructure;
using TransactionStore.Api.Models.OData;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Data;
using TransactionStore.Data.Entities;

namespace TransactionStore.Api.DomainEvents.Handlers
{
    public class PaymentTransactionGetFilteredHandler : IRequestHandler<PaymentTransactionsGetFilteredQuery, ODataResult<PaymentTransaction>
    >
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IODataTools _oDataTools;

        public PaymentTransactionGetFilteredHandler(AppDbContext dbContext, IMapper mapper, IODataTools oDataTools)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _oDataTools = oDataTools;
        }

        public async Task<ODataResult<PaymentTransaction>> Handle(PaymentTransactionsGetFilteredQuery request,
            CancellationToken cancellationToken)
        {
            var query = _mapper.Map<ExecutableODataQuery>(request.ODataQuery);
            var result = await _oDataTools.ODataResultAsync<PaymentTransactionEntity, PaymentTransaction>
                (_dbContext.Transactions.AsNoTracking(), query);

            return result;
        }
    }
}
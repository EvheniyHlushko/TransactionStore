using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TransactionStore.Data;
using TransactionStore.Data.Entities;

namespace TransactionStore.Api.DomainEvents.Notifications
{
    public class TransactionsParsedSaveToDb : INotificationHandler<TransactionsParsed>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TransactionsParsedSaveToDb(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(TransactionsParsed notification, CancellationToken cancellationToken)
        {
            var transactionEntities = _mapper.Map<IReadOnlyCollection<PaymentTransactionEntity>>(notification.Transactions);

            await _dbContext.BulkMergeAsync(transactionEntities, x =>
            {
                x.InsertIfNotExists = true;

                x.ColumnPrimaryKeyExpression = s => s.TransactionId;
            }, cancellationToken);
        }
    }
}
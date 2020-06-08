using System.Collections.Generic;
using MediatR;
using TransactionStore.Api.Models.Transaction;

namespace TransactionStore.Api.DomainEvents.Notifications
{
    public class TransactionsParsed : INotification
    {
        public TransactionsParsed(IReadOnlyCollection<PaymentTransaction> transactions)
        {
            Transactions = transactions;
        }

        public IReadOnlyCollection<PaymentTransaction> Transactions { get; }
    }
}
using System.Collections.Generic;

namespace TransactionStore.Api.Models.Transaction
{
    public class TransactionResponse
    {
        public IReadOnlyCollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
    }
}
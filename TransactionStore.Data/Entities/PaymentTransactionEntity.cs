using System;
using TransactionStore.Data.Enums;

namespace TransactionStore.Data.Entities
{
    public class PaymentTransactionEntity
    {
        public int Id { get; set; }

        public string TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
using System;
using TransactionStore.Data.Enums;

namespace TransactionStore.Api.Models.Transaction
{
    public class PaymentTransaction
    {
        public string TransactionId { get; set; } = default!;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = default!;

        public DateTime TransactionDate { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
using System;
using System.Collections.Generic;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.Models
{
    public class ParserResult<T>
    {
        public IReadOnlyCollection<T> ParsedTransactions { get; set; } = (IReadOnlyCollection<T>) Array.Empty<T>();

        public ValidationResult ValidationResult { get; set; } = default!;
    }
}
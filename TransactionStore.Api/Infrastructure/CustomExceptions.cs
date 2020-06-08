using System;
using System.Collections.Generic;
using System.Linq;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.Infrastructure
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, IReadOnlyList<ValidationFailure> errors)
        {
            Message = message;
            Errors = errors;
        }

        public ValidationException(ValidationResult validationResult)
        {
            Message = string.Empty;
            Errors = validationResult.Errors.ToList();
        }

        public override string Message { get; }

        public IReadOnlyList<ValidationFailure> Errors { get; }
    }
}
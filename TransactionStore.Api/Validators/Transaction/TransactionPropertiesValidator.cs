using System.Linq;
using System.Threading.Tasks;
using TransactionStore.Api.Models.Transaction;

namespace TransactionStore.Api.Validators.Transaction
{
    public class TransactionPropertiesValidator : IValidationService<FileParsingValidationContext<PaymentTransaction>>
    {
        public Task<ValidationResult> ValidateAsync(FileParsingValidationContext<PaymentTransaction> context)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(context.Model.TransactionId))
                result.Errors.Add(
                    GetCantBeNullFailure(nameof(PaymentTransaction.TransactionId), context.LineNumber, context.RawRecord));
            else if (context.Model.TransactionId.Length > 50)
                result.Errors.Add(new ValidationFailure
                {
                    PropertyName = nameof(PaymentTransaction.TransactionId),
                    ErrorMessage =
                        $"{nameof(PaymentTransaction.TransactionId)} length should not be greater than 50 characters. Line {context.LineNumber}",
                    RawRecord = context.RawRecord
                });

            if (string.IsNullOrEmpty(context.Model.Currency))
                result.Errors.Add(
                    GetCantBeNullFailure(nameof(PaymentTransaction.Currency), context.LineNumber, context.RawRecord));
            else if (!context.Model.Currency.All(char.IsLetterOrDigit) || context.Model.Currency.Length > 3)
                result.Errors.Add(new ValidationFailure
                {
                    PropertyName = nameof(PaymentTransaction.Currency),
                    ErrorMessage = $"Value {context.Model.Currency} has the wrong currency format. Line {context.LineNumber}",
                    RawRecord = context.RawRecord
                });

            return Task.FromResult(result);
        }


        private static ValidationFailure GetCantBeNullFailure(string property, int lineNumber, string rawRecord)
        {
            return new ValidationFailure
            {
                PropertyName = property,
                ErrorMessage = $"{property} can't be null or empty. Line {lineNumber}",
                RawRecord = rawRecord
            };
        }
    }
}
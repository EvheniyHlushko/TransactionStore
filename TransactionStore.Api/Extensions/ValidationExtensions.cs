using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.Extensions
{
    public static class ValidationExtensions
    {
        public static async Task HandleModelAsync<T>(
            this ISequencedValidationService<FileParsingValidationContext<T>> validationService,
            T model,
            int lineNumber,
            string rawRecord,
            List<ValidationFailure> errors,
            ICollection<T> models)
        {
            if (model == null) return;

            var validationResult = await validationService.Validate(
                new FileParsingValidationContext<T>(model, lineNumber, rawRecord));
            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors);
                return;
            }

            models.Add(model);
        }
    }
}
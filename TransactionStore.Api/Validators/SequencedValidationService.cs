using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransactionStore.Api.Validators
{
    public class SequencedValidationService<T> : ISequencedValidationService<T>
    {
        private readonly IEnumerable<IValidationService<T>> _validationServices;

        protected SequencedValidationService(IEnumerable<IValidationService<T>> validationServices)
        {
            _validationServices = validationServices;
        }

        public async Task<ValidationResult> Validate(T entity)
        {
            foreach (var validation in _validationServices)
            {
                var result = await validation.ValidateAsync(entity);
                if (!result.IsValid)
                    return result;
            }

            return new ValidationResult();
        }
    }
}
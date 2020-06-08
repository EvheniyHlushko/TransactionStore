using System.Threading.Tasks;

namespace TransactionStore.Api.Validators
{
    public abstract class ValidationService<T> : IValidationService<T>
    {
        public abstract Task<ValidationResult> ValidateAsync(T model);
    }
}
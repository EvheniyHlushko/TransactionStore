using System.Threading.Tasks;

namespace TransactionStore.Api.Validators
{
    public interface IValidationService<in T>
    {
        Task<ValidationResult> ValidateAsync(T model);
    }
}
using System.Threading.Tasks;

namespace TransactionStore.Api.Validators
{
    public interface ISequencedValidationService<in T>
    {
        Task<ValidationResult> Validate(T entity);
    }
}
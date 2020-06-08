using System.Collections.Generic;
using System.Linq;

namespace TransactionStore.Api.Validators
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<ValidationFailure>();
        }

        public ValidationResult(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures.Where(failure => failure != null).ToList();
        }


        public virtual bool IsValid => Errors.Count == 0;


        public IList<ValidationFailure> Errors { get; }
    }
}
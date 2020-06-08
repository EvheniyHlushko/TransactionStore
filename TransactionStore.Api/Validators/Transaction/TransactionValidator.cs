using System.Collections.Generic;
using TransactionStore.Api.Models.Transaction;

namespace TransactionStore.Api.Validators.Transaction
{
    public class TransactionValidator : SequencedValidationService<FileParsingValidationContext<PaymentTransaction>>
    {
        public TransactionValidator(IEnumerable<IValidationService<FileParsingValidationContext<PaymentTransaction>>> validationServices) :
            base(validationServices)
        {
        }
    }
}
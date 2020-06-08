using System.Globalization;
using CsvHelper.Configuration;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.Parsers.Transaction
{
    public class TransactionCsvParser : BaseCsvParser<PaymentTransaction>
    {
        public TransactionCsvParser(ISequencedValidationService<FileParsingValidationContext<PaymentTransaction>> validationService) : base(
            validationService)
        {
        }

        protected override CsvConfiguration GetConfiguration()
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                IgnoreQuotes = false,
                HasHeaderRecord = false,
                Quote = '"',
                TrimOptions = TrimOptions.Trim
            };
            configuration.RegisterClassMap<TransactionMap>();
            return configuration;
        }
    }
}
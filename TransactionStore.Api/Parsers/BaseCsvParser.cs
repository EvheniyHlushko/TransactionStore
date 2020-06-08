using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using TransactionStore.Api.Extensions;
using TransactionStore.Api.Models;
using TransactionStore.Api.Validators;
using ValidationException = TransactionStore.Api.Infrastructure.ValidationException;

namespace TransactionStore.Api.Parsers
{
    public abstract class BaseCsvParser<T> : IParser<T>
    {
        private readonly ISequencedValidationService<FileParsingValidationContext<T>> _validationService;

        protected BaseCsvParser(ISequencedValidationService<FileParsingValidationContext<T>> validationService)
        {
            _validationService = validationService;
        }

        public async Task<ParserResult<T>> ParseAsync(IFormFile file, CancellationToken token)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csvReader = new CsvReader(reader, GetConfiguration());

            var errors = new List<ValidationFailure>();

            csvReader.Configuration.ReadingExceptionOccurred = context =>
            {
                if (context.InnerException is ValidationException validationException) errors.AddRange(validationException.Errors);
                else
                {
                    errors.Add(new ValidationFailure()
                    {
                        ErrorMessage = $"Can't parse {file.FileName}. Line {context.ReadingContext.Row}",
                        PropertyName = "File",
                        RawRecord = context.ReadingContext.RawRecord
                    });
                }

                return false;
            };

            var models = new List<T>();

            while (csvReader.Read())
            {
                var model = csvReader.GetRecord<T>();

                await _validationService.HandleModelAsync(model, csvReader.Context.Row, csvReader.Context.RawRecord, errors, models);
            }

            return new ParserResult<T> {ParsedTransactions = models, ValidationResult = new ValidationResult(errors)};
        }

        public bool CanParse(string extension)
        {
            return extension.Equals(".csv");
        }

        protected abstract CsvConfiguration GetConfiguration();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using TransactionStore.Api.Extensions;
using TransactionStore.Api.Infrastructure;
using TransactionStore.Api.Models;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.Parsers
{
    public abstract class BaseXmlParser<T> : IParser<T>
    {
        private readonly ISequencedValidationService<FileParsingValidationContext<T>> _validationService;

        protected BaseXmlParser(ISequencedValidationService<FileParsingValidationContext<T>> validationService)
        {
            _validationService = validationService;
        }

        public async Task<ParserResult<T>> ParseAsync(IFormFile file, CancellationToken token)
        {

            try
            {
                var elements = await GetXElements(file, token);

                var errors = new List<ValidationFailure>();

                var models = new List<T>();

                foreach (var element in elements)
                {
                    if (!TryParseElement(element, out var item, errors)) continue;

                    await _validationService.HandleModelAsync(item, element.GetLineNumber(),
                        element.ToString(), errors, models);
                }

                return new ParserResult<T> {ParsedTransactions = models, ValidationResult = new ValidationResult(errors)};
            }
            catch (XmlException)
            {
                throw CantParseValidationException(file.FileName);
            }
        }

        private static async Task<IEnumerable<XElement>> GetXElements(IFormFile file, CancellationToken token)
        {
            await using var stream = file.OpenReadStream();
            
            var document = await XDocument.LoadAsync(stream, LoadOptions.SetLineInfo, token);

            var elements = document.Root?.Elements();

            if (elements == null)
            {
                throw CantParseValidationException(file.FileName);
            }

            return elements;
        }

        public bool CanParse(string extension)
        {
            return extension.Equals(".xml");
        }

        private static ValidationException CantParseValidationException(string fileName)
        {
            throw new ValidationException(new ValidationResult(new[]
            {
                new ValidationFailure
                {
                    PropertyName = "File",
                    ErrorMessage = $"Can't parse file {fileName}"
                }
            }));
        }

        protected abstract bool TryParseElement(XElement element, out T obj, ICollection<ValidationFailure> errors);
    }
}
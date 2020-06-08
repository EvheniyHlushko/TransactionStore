using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TransactionStore.Api.Infrastructure;
using TransactionStore.Api.Models;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.Parsers
{
    public class ParserStrategy<T> : IParserStrategy<T>
    {
        private readonly IEnumerable<IParser<T>> _parsers;

        public ParserStrategy(IEnumerable<IParser<T>> parsers)
        {
            _parsers = parsers;
        }

        public async Task<ParserResult<T>> ParseAsync(IFormFile file, CancellationToken token)
        {
            var extension = Path.GetExtension(file.FileName);
            var parser = _parsers.SingleOrDefault(p => p.CanParse(extension));

            if (parser != null) return await parser.ParseAsync(file, token);

            var errorMessage = $"File type isn't supported. FileName={file.FileName}";
            throw new ValidationException(errorMessage, new[]
            {
                new ValidationFailure
                {
                    ErrorMessage = errorMessage,
                    PropertyName = nameof(file)
                }
            });
        }
    }
}
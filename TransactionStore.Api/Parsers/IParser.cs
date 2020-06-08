using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TransactionStore.Api.Models;

namespace TransactionStore.Api.Parsers
{
    public interface IParser<T>
    {
        Task<ParserResult<T>> ParseAsync(IFormFile file, CancellationToken token);
        bool CanParse(string extension);
    }
}
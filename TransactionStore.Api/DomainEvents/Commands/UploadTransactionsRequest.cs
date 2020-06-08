using MediatR;
using Microsoft.AspNetCore.Http;
using TransactionStore.Api.Models.Transaction;

namespace TransactionStore.Api.DomainEvents.Commands
{
    public class UploadTransactionsRequest : IRequest<TransactionResponse>
    {
        public UploadTransactionsRequest(IFormFile file)
        {
            File = file;
        }

        public IFormFile File { get; }
    }
}
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ByteSizeLib;
using MediatR;
using Microsoft.Extensions.Options;
using TransactionStore.Api.DomainEvents.Commands;
using TransactionStore.Api.Infrastructure;
using TransactionStore.Api.Models;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.DomainEvents.PipelineBehaviors
{
    public class TransactionFileValidator : IPipelineBehavior<UploadTransactionsRequest, TransactionResponse>
    {
        private readonly FileRestrictionsSettings _fileRestrictionsSettings;

        public TransactionFileValidator(IOptions<FileRestrictionsSettings> options)
        {
            _fileRestrictionsSettings = options.Value;
        }

        public async Task<TransactionResponse> Handle(UploadTransactionsRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TransactionResponse> next)
        {
            if (!_fileRestrictionsSettings.SupportedFormats.Contains(Path.GetExtension(request.File.FileName)))
                throw new ValidationException("Unknown format", new[]
                {
                    new ValidationFailure
                    {
                        ErrorMessage = "Unknown format",
                        PropertyName = "File"
                    }
                });

            if (ByteSize.FromMegaBytes(_fileRestrictionsSettings.MaxSizeMb).Bytes < request.File.Length)
                throw new ValidationException("File exceeds the maximum file-size", new[]
                {
                    new ValidationFailure
                    {
                        ErrorMessage = $"File exceeds the maximum file size {_fileRestrictionsSettings.MaxSizeMb} MB",
                        PropertyName = "File"
                    }
                });

            return await next();
        }
    }
}
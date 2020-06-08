using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using TransactionStore.Api.Validators;

namespace TransactionStore.Api.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogError<T>(this ILogger<T> logger, IReadOnlyCollection<ValidationFailure> errors)
        {
            var errorMessageBuilder = new StringBuilder();

            foreach (var error in errors)
            {
                errorMessageBuilder.AppendLine(error.ErrorMessage);
                if (string.IsNullOrEmpty(error.RawRecord)) errorMessageBuilder.AppendLine($"Raw data: {error.RawRecord}");
            }

            logger.LogError(errorMessageBuilder.ToString());
        }
    }
}
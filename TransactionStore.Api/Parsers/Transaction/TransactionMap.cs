using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Api.Validators;
using TransactionStore.Data.Enums;
using ValidationException = TransactionStore.Api.Infrastructure.ValidationException;

namespace TransactionStore.Api.Parsers.Transaction
{
    internal sealed class TransactionMap : ClassMap<PaymentTransaction>
    {
        public TransactionMap()
        {
            Map(m => m.TransactionId).Index(0);
            Map(m => m.Amount).Index(1).ConvertUsing(ParseAmount);
            Map(m => m.Currency).Index(2);
            Map(m => m.Status).Index(4).ConvertUsing(ParseTransactionStatus);
            Map(m => m.TransactionDate).Index(3)
                .ConvertUsing(ParseTransactionDate);
        }

        private decimal ParseAmount(IReaderRow s)
        {
            var value = s.GetField(1).Replace(",", string.Empty);
            if (decimal.TryParse(value, out var result)) return result;

            throw new ValidationException(GetResultForNotParsedElement
                (nameof(PaymentTransaction.Amount), s.Context.Row, s.Context.RawRecord));
        }

        private DateTime ParseTransactionDate(IReaderRow s)
        {
            if (DateTime.TryParseExact(s.GetField(3), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var result)) return result;

            throw new ValidationException(GetResultForNotParsedElement
                (nameof(PaymentTransaction.TransactionDate), s.Context.Row, s.Context.RawRecord));
        }

        private static ValidationResult GetResultForNotParsedElement(string property, int line, string rawRecord)
        {
            return new ValidationResult(new[]
            {
                new ValidationFailure
                {
                    PropertyName = property,
                    ErrorMessage = $"Can't parse element. Line {line}",
                    RawRecord = rawRecord
                }
            });
        }

        private TransactionStatus ParseTransactionStatus(IReaderRow s)
        {
            return s.GetField(4) switch
            {
                "Approved" => TransactionStatus.A,
                "Failed" => TransactionStatus.R,
                "Finished" => TransactionStatus.D,
                _ => throw new ValidationException(GetResultForNotParsedElement
                    (nameof(PaymentTransaction.Status), s.Context.Row, s.Context.RawRecord))
            };
        }
    }
}
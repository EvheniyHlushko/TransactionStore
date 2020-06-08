using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TransactionStore.Api.Consts;
using TransactionStore.Api.Extensions;
using TransactionStore.Api.Models.Transaction;
using TransactionStore.Api.Validators;
using TransactionStore.Data.Enums;

namespace TransactionStore.Api.Parsers.Transaction
{
#nullable disable warnings
    public class TransactionXmlParser : BaseXmlParser<PaymentTransaction>
    {
        public TransactionXmlParser(ISequencedValidationService<FileParsingValidationContext<PaymentTransaction>> validationService) : base(
            validationService)
        {
        }

        protected override bool TryParseElement(XElement element, out PaymentTransaction? obj, ICollection<ValidationFailure> errors)
        {
            var dict = new Dictionary<string, XObject?>
            {
                {TransactionXmlConsts.TransactionDate, element.Element(TransactionXmlConsts.TransactionDate)},
                {TransactionXmlConsts.Status, element.Element(TransactionXmlConsts.Status)},
                {
                    TransactionXmlConsts.CurrencyCode, element.Element(TransactionXmlConsts.PaymentDetails)?
                        .Element(TransactionXmlConsts.CurrencyCode)
                },
                {
                    TransactionXmlConsts.Amount, element.Element(TransactionXmlConsts.PaymentDetails)?
                        .Element(TransactionXmlConsts.Amount)
                },
                {TransactionXmlConsts.TransactionId, element.Attribute(TransactionXmlConsts.TransactionId)}
            };


            if (IsElementNull(dict.ToArray(), element.GetLineNumber(), errors) ||
                !TryParseElements(dict, out var parsedElements, errors))
            {
                obj = null;
                return false;
            }

            var model = new PaymentTransaction
            {
                TransactionId = ((XAttribute) dict[TransactionXmlConsts.TransactionId]).Value,
                TransactionDate = parsedElements.ParsedDate,
                Status = parsedElements.parsedStatus,
                Currency = ((XElement) dict[TransactionXmlConsts.CurrencyCode]).Value,
                Amount = parsedElements.ParsedAmount
            };

            obj = model;
            return true;
        }


        private static bool TryParseElements(IReadOnlyDictionary<string, XObject?> dict,
            out (DateTime ParsedDate, decimal ParsedAmount, TransactionStatus parsedStatus) parsedElements,
            ICollection<ValidationFailure> errors)
        {
            var canParse = true;
            var dateElement = (XElement) dict[TransactionXmlConsts.TransactionDate];
            if (!DateTime.TryParse(dateElement.Value, out var parsedDate))
            {
                errors.Add(CreateCantParseFailure(dateElement, TransactionXmlConsts.TransactionDate));
                canParse = false;
            }

            var amountElement = (XElement) dict[TransactionXmlConsts.Amount];
            if (!decimal.TryParse(amountElement.Value, out var parsedAmount))
            {
                errors.Add(CreateCantParseFailure(amountElement, TransactionXmlConsts.Amount));
                canParse = false;
            }

            var statusElement = (XElement) dict[TransactionXmlConsts.Status];
            if (!TryGetTransactionStatus(statusElement.Value, out var parsedStatus))
            {
                errors.Add(CreateCantParseFailure(statusElement, TransactionXmlConsts.Status));
                canParse = false;
            }

            parsedElements = (parsedDate, parsedAmount, parsedStatus);
            return canParse;
        }

        private static ValidationFailure CreateCantParseFailure(XElement element, string elementName)
        {
            return new ValidationFailure
            {
                PropertyName = elementName,
                ErrorMessage = $"Can't parse {element} element. Line {element.GetLineNumber()}",
                RawRecord = element.ToString()
            };
        }

        private static bool IsElementNull(IEnumerable<KeyValuePair<string, XObject?>> elements, int parentLine,
            ICollection<ValidationFailure> errors)
        {
            var isAnyNull = false;

            foreach (var (key, value) in elements)
            {
                if (value != null) continue;
                errors.Add(new ValidationFailure
                {
                    ErrorMessage = $"Can't find element {key}, parent line: {parentLine}"
                });
                isAnyNull = true;
            }

            return isAnyNull;
        }
        
        private static bool TryGetTransactionStatus(string value, out TransactionStatus status)
        {
            switch (value)
            {
                case "Approved":
                {
                    status = TransactionStatus.A;
                    return true;
                }
                case "Rejected":
                {
                    status = TransactionStatus.R;
                    return true;
                }
                case "Done":
                {
                    status = TransactionStatus.D;
                    return true;
                }
                default:
                {
                    status = 0;
                    return false;
                }
            }
        }
    }
}
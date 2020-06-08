namespace TransactionStore.Api.Validators
{
    public class FileParsingValidationContext<T>
    {
        public FileParsingValidationContext(T model, int lineNumber, string rawRecord)
        {
            Model = model;
            LineNumber = lineNumber;
            RawRecord = rawRecord;
        }

        public T Model { get; }
        public int LineNumber { get; }

        public string RawRecord { get; }
    }
}
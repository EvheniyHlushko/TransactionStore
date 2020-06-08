namespace TransactionStore.Api.Models.OData
{
    public class ExecutableODataQuery
    {
        public string? Filter { get; set; }

        public string? Skip { get; set; }

        public string? Top { get; set; }

        public string? OrderBy { get; set; }
    }
}
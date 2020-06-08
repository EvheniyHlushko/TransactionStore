namespace TransactionStore.Api.Models.OData
{
    public class ODataQuery
    {
        [SwaggerODataFilter("$filter", "Filter the results using OData syntax.")]
        public string? Filter { get; set; }


        [SwaggerODataFilter("$skip", "The number of results to skip.")]
        public string? Skip { get; set; }


        [SwaggerODataFilter("$top", "The number of results to return.")]
        public string? Top { get; set; }


        [SwaggerODataFilter("$orderby", "Order the results using OData syntax.")]
        public string? OrderBy { get; set; }
    }
}
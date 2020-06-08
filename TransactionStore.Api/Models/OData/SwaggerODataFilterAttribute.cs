using Microsoft.AspNetCore.Mvc;

namespace TransactionStore.Api.Models.OData
{
    public class SwaggerODataFilterAttribute : FromQueryAttribute
    {
        public SwaggerODataFilterAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Description { get; }
    }
}
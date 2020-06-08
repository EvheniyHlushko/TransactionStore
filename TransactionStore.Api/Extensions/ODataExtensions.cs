using System.Linq;
using Community.OData.Linq;
using Community.OData.Linq.OData.Query;
using TransactionStore.Api.Models.OData;

namespace TransactionStore.Api.Extensions
{
    public static class ODataExtensions
    {
        public static IQueryable<T> ApplyOData<T>(this IQueryable<T> source, ExecutableODataQuery query)
        {
            return source.OData().ApplyQueryOptionsWithoutSelectExpand(new ODataRawQueryOptions
            {
                Filter = query.Filter,
                OrderBy = query.OrderBy,
                Skip = query.Skip,
                Top = query.Top,
                Select = null,
                Expand = null
            });
        }


        public static IQueryable<T> ApplyOData<T>(this IQueryable<T> source, ExecutableODataQuery query, int maxNodeCount)
        {
            return source.OData(x => x.ValidationSettings.MaxNodeCount = maxNodeCount).ApplyQueryOptionsWithoutSelectExpand(
                new ODataRawQueryOptions
                {
                    Filter = query.Filter,
                    OrderBy = query.OrderBy,
                    Skip = query.Skip,
                    Top = query.Top,
                    Select = null,
                    Expand = null
                });
        }

        public static IQueryable<T> ApplyODataCaseInsensitive<T>(this IQueryable<T> source, ExecutableODataQuery query)
        {
            return source.OData(x => x.EnableCaseInsensitive = true).ApplyQueryOptionsWithoutSelectExpand(new ODataRawQueryOptions
            {
                Filter = query.Filter,
                OrderBy = query.OrderBy,
                Skip = query.Skip,
                Top = query.Top,
                Select = null,
                Expand = null
            });
        }
    }
}
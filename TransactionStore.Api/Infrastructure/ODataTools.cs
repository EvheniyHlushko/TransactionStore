using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TransactionStore.Api.Extensions;
using TransactionStore.Api.Models.OData;

namespace TransactionStore.Api.Infrastructure
{
    public class ODataTools : IODataTools
    {
        private readonly IMapper _mapper;

        public ODataTools(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ODataResult<TDto>> ODataResultAsync<TEntity, TDto>(IQueryable<TEntity> data,
            ExecutableODataQuery odataQuery)
            where TEntity : class
            where TDto : class
        {
            var maxNodeCount = 1000;

            data = IsInMemoryCollection(data) ? data : data.AsNoTracking();

            var count = await GetCountAsync(data, odataQuery, maxNodeCount);
            var items = data.ApplyOData(odataQuery, maxNodeCount).ToArray();

            return GetResult<TEntity, TDto>(count, items);
        }

        private ODataResult<TDto> GetResult<TEntity, TDto>(int count, TEntity[] items)
            where TEntity : class
            where TDto : class
        {
            return new ODataResult<TDto>
            {
                Count = count,
                Items = _mapper.Map<List<TDto>>(items)
            };
        }

        private static async Task<int> GetCountAsync<TEntity>(IQueryable<TEntity> data,
            ExecutableODataQuery odataQuery, int maxNodeCount = 100) where TEntity : class
        {
            var ignorePageQuery = new ExecutableODataQuery
            {
                Filter = odataQuery.Filter,
                Top = int.MaxValue.ToString()
            };

            var odata = data.ApplyOData(ignorePageQuery, maxNodeCount);
            return await (IsInMemoryCollection(data) ? Task.FromResult(odata.Count()) : odata.CountAsync());
        }

        private static bool IsInMemoryCollection<TEntity>(IQueryable<TEntity> data) where TEntity : class
        {
            return data.GetType() == typeof(EnumerableQuery<TEntity>);
        }
    }
}
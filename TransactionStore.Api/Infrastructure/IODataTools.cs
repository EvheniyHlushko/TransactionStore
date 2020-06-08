using System.Linq;
using System.Threading.Tasks;
using TransactionStore.Api.Models.OData;

namespace TransactionStore.Api.Infrastructure
{
    public interface IODataTools
    {
        Task<ODataResult<TDto>> ODataResultAsync<TEntity, TDto>(IQueryable<TEntity> data,
            ExecutableODataQuery odataQuery)
            where TEntity : class
            where TDto : class;
    }
}
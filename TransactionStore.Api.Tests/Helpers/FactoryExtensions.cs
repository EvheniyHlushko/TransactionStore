using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TransactionStore.Api.Tests.Helpers
{
    public static class FactoryExtensions
    {
        public static async Task PopulateContextAsync<T>(this TransactionStoreWebFactory factory, T model) where T : class
        {
            factory.Context!.Set<T>().Add(model);
            await factory.Context!.SaveChangesAsync();
        }

        public static void DetachAllEntities(this TransactionStoreWebFactory factory)
        {
            var entries = factory.Context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries)
                if (entry.Entity != null)
                    entry.State = EntityState.Detached;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TransactionStore.Data;

namespace TransactionStore.Api.Tests.Helpers
{
    public class TestDbContext : AppDbContext
    {
        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
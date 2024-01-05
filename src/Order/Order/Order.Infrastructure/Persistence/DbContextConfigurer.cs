using Microsoft.EntityFrameworkCore;

namespace Order.Infrastructure.Persistence
{
    internal class DbContextConfigurer
    {

        private static string _connectionString = "User";
        public static void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<OrderDbContext> optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_connectionString);
        }
    }
}
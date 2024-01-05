using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Persistence
{
    internal class DbContextConfigurer
    {

        private static string _connectionString = "User";
        public static void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ProductDbContext> optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_connectionString);
        }
    }
}
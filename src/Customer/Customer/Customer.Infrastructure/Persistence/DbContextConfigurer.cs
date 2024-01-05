using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Persistence
{
    internal class DbContextConfigurer
    {

        private static string _connectionString = "User";
        public static void Configure(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CustomerDbContext> optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_connectionString);
        }
    }
}
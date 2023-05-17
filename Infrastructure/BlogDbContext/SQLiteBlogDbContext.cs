using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BlogDbContext
{
    public class SQLiteBlogDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer() //TODO: SQLite connection
        }
    }
}

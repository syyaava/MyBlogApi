using BlogCore.Blog;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BlogDbContext
{
    public class SQLiteBlogDbContext : DbContext, IDbContext<BlogMessage>
    {
        private DbSet<BlogMessage> messages => Set<BlogMessage>();

        public IEnumerable<BlogMessage> Values
        {
            get => messages;
        }

        public SQLiteBlogDbContext()
        {
            Database.EnsureCreated();    
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Blog.db");
        }
    }
}

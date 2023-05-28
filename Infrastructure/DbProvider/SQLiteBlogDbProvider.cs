using BlogCore.Blog;
using BlogCore.Db;
using Infrastructure.BlogDbContext;

namespace Infrastructure.DbProvider
{
    public class SQLiteBlogMessageDbProvider : IBlogDbProvider
    {
        private IDbContext<BlogMessage> dbContext;

        public SQLiteBlogMessageDbProvider(IDbContext<BlogMessage> dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<BlogMessage> AddMessages(params BlogMessage[] message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlogMessage> GetAllUserMessages(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BlogMessage> GetLastUserMessages(string userId, int count)
        {
            throw new NotImplementedException();
        }

        public BlogMessage GetMessage(string id)
        {
            throw new NotImplementedException();
        }

        public BlogMessage RemoveMessage(string id)
        {
            throw new NotImplementedException();
        }

        public BlogMessage UpdateMessage(string id, BlogMessage newMessage)
        {
            throw new NotImplementedException();
        }
    }
}

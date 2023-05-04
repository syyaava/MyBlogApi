using BlogCore.Blog;
using Infrastructure.DbActionResults;

namespace Infrastructure
{
    public class SQLiteBlogDbProvider : IBlogDbProvider
    {
        public DbActionResult<IEnumerable<BlogMessage>> AddMessages(params BlogMessage[] message)
        {
            throw new NotImplementedException();
        }

        public DbActionResult<IEnumerable<BlogMessage>> GetAllUserMessages(string userId)
        {
            throw new NotImplementedException();
        }

        public DbActionResult<IEnumerable<BlogMessage>> GetLastUserMessages(string userId, int count)
        {
            throw new NotImplementedException();
        }

        public DbActionResult<BlogMessage> GetMessage(string id)
        {
            throw new NotImplementedException();
        }

        public DbActionResult<BlogMessage> RemoveMessage(string id)
        {
            throw new NotImplementedException();
        }

        public DbActionResult<BlogMessage> UpdateMessage(string id, BlogMessage newMessage)
        {
            throw new NotImplementedException();
        }
    }
}

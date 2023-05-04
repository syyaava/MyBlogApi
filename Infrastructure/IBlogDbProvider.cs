using BlogCore.Blog;
using Infrastructure.DbActionResults;

namespace Infrastructure
{
    public interface IBlogDbProvider
    {
        public DbActionResult<BlogMessage> GetMessage(string id);
        public DbActionResult<IEnumerable<BlogMessage>> GetAllUserMessages(string userId);
        public DbActionResult<IEnumerable<BlogMessage>> GetLastUserMessages(string userId, int count);
        public DbActionResult<IEnumerable<BlogMessage>> AddMessages(params BlogMessage[] message);
        public DbActionResult<BlogMessage> RemoveMessage(string id);
        public DbActionResult<BlogMessage> UpdateMessage(string id, BlogMessage newMessage);
    }
}

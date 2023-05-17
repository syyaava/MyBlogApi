using BlogCore.Blog;

namespace BlogCore.Db
{
    public interface IBlogDbProvider
    {
        public BlogMessage GetMessage(string id);
        public IEnumerable<BlogMessage> GetAllUserMessages(string userId);
        public IEnumerable<BlogMessage> GetLastUserMessages(string userId, int count);
        public IEnumerable<BlogMessage> AddMessages(params BlogMessage[] message);
        public BlogMessage RemoveMessage(string id);
        public BlogMessage UpdateMessage(string id, BlogMessage newMessage);
    }
}

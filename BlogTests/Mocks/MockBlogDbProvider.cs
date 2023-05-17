using BlogCore.Blog;
using BlogCore.Db;
using BlogCore.Exceptions;

namespace BlogTests.Mocks
{
    public class MockBlogDbProvider : IBlogDbProvider
    {
        private List<BlogMessage> messages;

        public MockBlogDbProvider()
        {
            messages = new List<BlogMessage>();    
        }

        public IEnumerable<BlogMessage> AddMessages(params BlogMessage[] messages)
        {
            if (messages == null)
                throw new ArgumentNullException();

            if (messages.Length == 0)
                throw new ArgumentException();

            foreach(var message in messages)
            {
                if(this.messages.Where(m => m.Id == message.Id).Count() > 0)
                    throw new AlreadyExistException();

                this.messages.Add(message);
            }

            return messages;
        }

        public IEnumerable<BlogMessage> GetAllUserMessages(string userId)
        {
            ValidateId(userId);
            var allMessages = messages.Where(message => message.UserId == userId);
            if(allMessages.Count() == 0)
                throw new NotFoundException<string>();
            return allMessages;
        }

        public IEnumerable<BlogMessage> GetLastUserMessages(string userId, int count)
        {
            ValidateId(userId);
            var lastMessages = messages.Where(message => message.UserId == userId);
            if (lastMessages.Count() == 0)
                throw new NotFoundException<string>();
            return lastMessages;
        }

        public BlogMessage GetMessage(string id)
        {
            ValidateId(id);
            var message = messages.FirstOrDefault(message => message.Id == id);
            return message == null ? throw new NotFoundInDbException<string>() : message;
        }

        public BlogMessage RemoveMessage(string id)
        {
            ValidateId(id);
            var message = messages.FirstOrDefault(message => message.Id == id);
            if(message == null)
                throw new NotFoundInDbException<string>();

            messages.Remove(message);
            return message;
        }

        public BlogMessage UpdateMessage(string id, BlogMessage newMessage)
        {
            ValidateId(id);
            ValidateId(newMessage.Id.ToString());
            var message = messages.FirstOrDefault(message => message.Id == id);
            if(message == null)
                throw new NotFoundInDbException<string>();

            var index = messages.IndexOf(message);
            messages[index] = newMessage;
            return newMessage;
        }

        private void ValidateId(string id)
        {
            if (id == null) throw new ArgumentNullException();
            if (string.IsNullOrEmpty(id)) throw new ArgumentException();
        }
    }
}

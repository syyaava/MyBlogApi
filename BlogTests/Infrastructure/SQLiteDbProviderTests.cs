using BlogCore.Blog;
using BlogCore.Db;
using BlogCore.Exceptions;
using Infrastructure;
using Infrastructure.BlogDbContext;
using Infrastructure.DbProvider;

namespace BlogTests.Infrastructure
{
    public class SQLiteDbProviderTests
    {
        private IBlogDbProvider dbProvider;
        private readonly List<User> users = new List<User>();

        public SQLiteDbProviderTests()
        {
            InitializeDbProvider();
            users.Add(new User("user1", "user1@mail.ru"));
            users.Add(new User("user2", "user2@mail.ru"));
            users.Add(new User("user3", "user3@mail.ru"));
            users.Add(new User("user4", "user4@mail.ru"));
            users.Add(new User("user5", "user5@mail.ru"));
            users.Add(new User("admin", "admin@mail.ru"));
        }

        private void InitializeDbProvider()
        {
            IDbContext<BlogMessage> dbContext = null;
            dbProvider = new SQLiteBlogMessageDbProvider();
        }

        [Fact]
        public void GetMessage_ExistingMessage_ReturnBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "My first message. Hello everyone.");
            dbProvider.AddMessages(message);

            var messageFromDb = dbProvider.GetMessage(message.Id);

            Assert.NotNull(messageFromDb);
            Assert.Equal(user.Id, messageFromDb.UserId);
            Assert.Equal(message.Message, messageFromDb.Message);
            Assert.Equal(message.CreationTime, messageFromDb.CreationTime);
            Assert.Equal(message.UpdateTime, messageFromDb.UpdateTime);
        }

        [Fact]
        public void GetMessage_NotExistingMessage_ThrowNotFoundInDbException()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "My first message. Hello everyone.");

            Assert.Throws<NotFoundInDbException<string>>(() => dbProvider.GetMessage(message.Id));
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void GetMessage_NotValidId_ThrowException(string? id, Type exType)
        {
            InitializeDbProvider();

            Assert.Throws(exType, () => dbProvider.GetMessage(id));
        }

        [Fact]
        public void GetAllUserMessages_ExistingUserIdMessages_ReturnIEnumnerableBlogMessages()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            List<BlogMessage> messages = CreateMessageListFor(user1, user2);
            dbProvider.AddMessages(messages.ToArray());

            var messagesFromDb = dbProvider.GetAllUserMessages(user1.Id);

            Assert.NotNull(messagesFromDb);
            Assert.NotEmpty(messagesFromDb);
            Assert.Equal(messages.Count(x => x.UserId == user1.Id), messagesFromDb.Count());
            AllMessagesExistsIn(messages, messagesFromDb);
        }

        private List<BlogMessage> CreateMessageListFor(User user1, User user2)
        {
            return new List<BlogMessage>()
            {
                new BlogMessage(user1.Id, "My first blog message."),
                new BlogMessage(user1.Id, "My second blog message. I cry."),
                new BlogMessage(user1.Id, "My third blog message. I have fun."),
                new BlogMessage(user2.Id, "First message in thi blog. Subscribe and have fun."),
            };
        }

        private void AllMessagesExistsIn(List<BlogMessage> expected, IEnumerable<BlogMessage> actual)
        {
            foreach (var message in actual)
                Assert.True(expected.FirstOrDefault(x => x.Id == message.Id &&
                x.UserId == message.UserId &&
                x.Message == message.Message &&
                x.CreationTime == message.CreationTime &&
                x.UpdateTime == message.UpdateTime) != null);
        }

        [Fact]
        public void GetAllUserMessages_NotExistingUserIdMessages_ThrowNotFoundInDbException()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            var messages = CreateMessageListFor(user1, user2);
            Assert.Throws<NotFoundInDbException<string>>( () => dbProvider.GetAllUserMessages(user1.Id));
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void GetAllUserMessages_NotValidUserId_ThrowException(string? userId, Type exType)
        {
            InitializeDbProvider();

            Assert.Throws(exType, () => dbProvider.GetAllUserMessages(userId));
        }

        [Fact]
        public void GetLastUserMessages_ExistingUserIdValidCount_ReturnIEnumerableBlogMessages()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            var messageCount = 2;
            var messages = CreateMessageListFor(user1, user2);
            dbProvider.AddMessages(messages.ToArray());

            var messagesFromDb = dbProvider.GetLastUserMessages(user1.Id, messageCount);

            Assert.NotNull(messagesFromDb);
            Assert.NotEmpty(messagesFromDb);
            Assert.True(messageCount >= messagesFromDb.Count());
            AllMessagesExistsIn(messages, messagesFromDb);
        }

        [Fact]
        public void GetLastUserMessages_NotExistingUserIdValidCount_ReturnEmptyIEnumerableBlogMessage()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            var messageCount = 2;
            var messages = CreateMessageListFor(user1, user2);
            dbProvider.AddMessages(messages.ToArray());

            var messagesFromDb = dbProvider.GetLastUserMessages(user1.Id, messageCount);

            Assert.NotNull(messagesFromDb);
            Assert.Empty(messagesFromDb);
        }

        [Theory]
        [InlineData("user-F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4", -555, typeof(ArgumentOutOfRangeException))]
        [InlineData("", 22, typeof(ArgumentException))]
        [InlineData("", -55, typeof(ArgumentException))]
        [InlineData(null, 22, typeof(ArgumentNullException))]
        [InlineData(null, -55, typeof(ArgumentNullException))]
        public void GetLastUserMessages_NotValidParametrs_ThrowException(string? userId, int messageCount, Type exType)
        {
            InitializeDbProvider();

            Assert.Throws(exType, () => dbProvider.GetLastUserMessages(userId, messageCount));
        }

        [Fact]
        public void AddMessages_OneMessage_ReturnAddedIEnumerableBlogMessageWithOneObject()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var message = new BlogMessage(user1.Id, "This is message");

            var resultOfAdding = dbProvider.AddMessages(message);

            Assert.NotNull(resultOfAdding);

            Assert.Equal(message.Id, resultOfAdding.ElementAt(0).Id);
            Assert.Equal(message.UserId, resultOfAdding.ElementAt(0).UserId);
            Assert.Equal(message.Message, resultOfAdding.ElementAt(0).Message);
            Assert.Equal(message.CreationTime, resultOfAdding.ElementAt(0).CreationTime);
        }

        [Fact]
        public void AddMessages_AlreadyExistingMessage_ThrowAlreadyExistException()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var message = new BlogMessage(user1.Id, "This is message");

            dbProvider.AddMessages(message);
            Assert.Throws<AlreadyExistException>(() => dbProvider.AddMessages(message));
        }

        [Fact]
        public void AddMessage_ManyMessages_ReturnIEnumerableBlogMessages()
        {
            InitializeDbProvider();
            var user = users[0];
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My 1 message."),
                new BlogMessage(user.Id, "My second message. Hahaha"),
                new BlogMessage(user.Id, "My third message. HOHOHOHOHO")
            };

            var resultOfAdding = dbProvider.AddMessages(messages.ToArray());

            Assert.NotNull(resultOfAdding);
            Assert.NotEmpty(resultOfAdding);
            AllMessagesExistsIn(messages, resultOfAdding);
        }

        [Fact]
        public void AddMessage_EmptyMessageCollection_ThrowArgumentException()
        {
            InitializeDbProvider();
            var messages = new List<BlogMessage>();

            Assert.Throws<ArgumentException>( () => dbProvider.AddMessages(messages.ToArray()));
        }

        [Fact]
        public void AddMessage_NullCollection_ThrowArgumentNullException()
        {
            InitializeDbProvider();

            Assert.Throws<ArgumentNullException>(() => dbProvider.AddMessages(null));
        }

        [Fact]
        public void RemoveMessage_ExistingMessage_ReturnDeletedBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "MESSAGE");

            dbProvider.AddMessages(message);
            var removeResult = dbProvider.RemoveMessage(message.Id);

            Assert.NotNull(removeResult);
            Assert.Equal(message.Id, removeResult.Id);
            Assert.Equal(message.UserId, removeResult.UserId);
            Assert.Equal(message.Message, removeResult.Message);
            Assert.Equal(message.CreationTime, removeResult.CreationTime);
            Assert.Equal(message.UpdateTime, removeResult.UpdateTime);
        }

        [Fact]
        public void RemoveMessage_NotExistingMessage_ThrowNotFoundInDbException()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "MESSAGE");

            Assert.Throws<NotFoundException<string>>( () => dbProvider.RemoveMessage(message.Id));
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void RemoveMessage_NotValidMessageId_ThrowException(string? messageId, Type exType)
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "MESSAGE");

            dbProvider.AddMessages(message);
            Assert.Throws(exType, () => dbProvider.RemoveMessage(messageId));
        }

        [Fact]
        public void UpdateMessage_ExistingId_ReturnUpdatedBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");
            var newMessage = new BlogMessage(user.Id, "new MESSAGE");

            dbProvider.AddMessages(message);
            var updateResult = dbProvider.UpdateMessage(message.Id, newMessage);

            Assert.NotNull(updateResult);
            Assert.Equal(updateResult.Message, newMessage.Message);
            Assert.Equal(updateResult.Id, message.Id);
            Assert.Equal(updateResult.UserId, message.UserId);
        }

        [Fact]
        public void UpdateMessage_NotExistingId_ThrowNotFoundInDbException()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");
            var newMessage = new BlogMessage(user.Id, "new MESSAGE");

            Assert.Throws<NotFoundException<string>>( () => dbProvider.UpdateMessage(message.Id.ToString(), newMessage));
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void UpdateMessage_NotValidMessageId_ThrowException(string? messageId, Type exType)
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");
            var newMessage = new BlogMessage(user.Id, "new MESSAGE");

            Assert.Throws(exType, () => dbProvider.UpdateMessage(messageId, newMessage));
        }

        [Fact]
        public void UpdateMessage_NullNewMessage_ThrowArgumentNullException()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");

            Assert.Throws<ArgumentNullException>( () => dbProvider.UpdateMessage(message.Id.ToString(), null));
        }
    }
}

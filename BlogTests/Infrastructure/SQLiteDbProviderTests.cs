using BlogCore.Blog;
using Infrastructure;
using Infrastructure.DbActionResults;

namespace BlogTests.Infrastructure
{
    public class SQLiteDbProviderTests
    {
        private SQLiteBlogDbProvider dbProvider;
        private List<User> users = new List<User>();
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
            dbProvider = new SQLiteBlogDbProvider();
        }

        [Fact]
        public void GetMessage_ExistingMessage_ReturnSuccessDbActionResultWithBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "My first message. Hello everyone.");
            dbProvider.AddMessages(message);

            var messageFromDb = dbProvider.GetMessage(message.Id.ToString());

            Assert.NotNull(messageFromDb.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.Success, messageFromDb.Status);
            Assert.Null(messageFromDb.Exception);
            Assert.Equal(user.Id, messageFromDb.Result.UserId);
            Assert.Equal(message.Message, messageFromDb.Result.Message);
        }

        [Fact]
        public void GetMessage_NotExistingMessage_ReturnNotFoundDbActionResultWithoutBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "My first message. Hello everyone.");

            var messageFromDb = dbProvider.GetMessage(message.Id.ToString());

            Assert.NotNull(messageFromDb.Result);
            Assert.Null(messageFromDb.Exception);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.NotFound, messageFromDb.Status);
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void GetMessage_NotValidId_ReturnErrorWithException(string? id, Type exType)
        {
            InitializeDbProvider();

            var messageFromDb = dbProvider.GetMessage(id);

            Assert.Null(messageFromDb.Result);
            Assert.NotNull(messageFromDb.Exception);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.Error, messageFromDb.Status);
            Assert.Equal(exType, messageFromDb.Exception.GetType());
        }

        [Fact]
        public void GetAllUserMessages_ExistingUserIdMessages_ReturnSuccessDbActionResultWithIEnumnerableBlogMessages()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user1.Id, "My first blog message."),
                new BlogMessage(user1.Id, "My second blog message. I cry."),
                new BlogMessage(user1.Id, "My third blog message. I have fun."),
                new BlogMessage(user2.Id, "First message in thi blog. Subscribe and have fun."),
            };
            dbProvider.AddMessages(messages.ToArray());

            var messagesFromDb = dbProvider.GetAllUserMessages(user1.Id);

            Assert.NotNull(messagesFromDb);
            Assert.Null(messagesFromDb.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Success, messagesFromDb.Status);
            Assert.NotNull(messagesFromDb.Result);
            Assert.NotEmpty(messagesFromDb.Result);
            Assert.Equal(messages.Count(x => x.UserId == user1.Id), messagesFromDb.Result.Count());
            foreach (var message in messagesFromDb.Result)
                Assert.True(messages.FirstOrDefault(x => x.Id == message.Id && x.UserId == message.UserId) != null);
        }

        [Fact]
        public void GetAllUserMessages_NotExistingUserIdMessages_ReturnNotFoundDbActionResultWithEmptyIEnumnerable()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user1.Id, "My first blog message."),
                new BlogMessage(user1.Id, "My second blog message. I cry."),
                new BlogMessage(user1.Id, "My third blog message. I have fun."),
                new BlogMessage(user2.Id, "First message in thi blog. Subscribe and have fun."),
            };

            var messagesFromDb = dbProvider.GetAllUserMessages(user1.Id);

            Assert.NotNull(messagesFromDb);
            Assert.Null(messagesFromDb.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.NotFound, messagesFromDb.Status);
            Assert.NotNull(messagesFromDb.Result);
            Assert.Empty(messagesFromDb.Result);    
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void GetAllUserMessages_NotValidUserId_ReturnErrorDbActionResultWithException(string? userId, Type exType)
        {
            InitializeDbProvider();

            var messagesFromDb = dbProvider.GetAllUserMessages(userId);

            Assert.NotNull(messagesFromDb);
            Assert.NotNull(messagesFromDb.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Error, messagesFromDb.Status);
            Assert.Equal(exType, messagesFromDb.Exception.GetType());  
        }

        [Fact]
        public void GetLastUserMessages_ExistingUserIdValidCount_ReturnSuccessDbActionResultWithIEnumerableBlogMessages()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            var messageCount = 2;
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user1.Id, "My first blog message."),
                new BlogMessage(user1.Id, "My second blog message. I cry."),
                new BlogMessage(user1.Id, "My third blog message. I have fun."),
                new BlogMessage(user2.Id, "First message in thi blog. Subscribe and have fun."),
            };
            dbProvider.AddMessages(messages.ToArray());

            var messagesFromDb = dbProvider.GetLastUserMessages(user1.Id, messageCount);

            Assert.NotNull(messagesFromDb);
            Assert.Null(messagesFromDb.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Success, messagesFromDb.Status);
            Assert.NotNull(messagesFromDb.Result);
            Assert.NotEmpty(messagesFromDb.Result);
            Assert.True(messageCount >= messagesFromDb.Result.Count());
            foreach (var message in messagesFromDb.Result)
                Assert.True(messages.FirstOrDefault(x => x.Id == message.Id && x.UserId == message.UserId) != null);
        }

        [Fact]
        public void GetLastUserMessages_NotExistingUserIdValidCount_ReturnSuccessDbActionResultWithEmptyIEnumerable()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var user2 = users[1];
            var messageCount = 2;
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user1.Id, "My first blog message."),
                new BlogMessage(user1.Id, "My second blog message. I cry."),
                new BlogMessage(user1.Id, "My third blog message. I have fun."),
                new BlogMessage(user2.Id, "First message in thi blog. Subscribe and have fun."),
            };
            dbProvider.AddMessages(messages.ToArray());

            var messagesFromDb = dbProvider.GetLastUserMessages(user1.Id, messageCount);

            Assert.NotNull(messagesFromDb);
            Assert.Null(messagesFromDb.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.NotFound, messagesFromDb.Status);
            Assert.NotNull(messagesFromDb.Result);
            Assert.Empty(messagesFromDb.Result);
        }

        [Theory]
        [InlineData("user-F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4", -555, typeof(ArgumentOutOfRangeException))]
        [InlineData("", 22, typeof(ArgumentException))]
        [InlineData("", -55, typeof(ArgumentException))]
        [InlineData(null, 22, typeof(ArgumentNullException))]
        [InlineData(null, -55, typeof(ArgumentNullException))]
        public void GetLastUserMessages_NotValidParametrs_ReturnErrorDbActionResultWithException(string? userId, int messageCount, Type exType)
        {
            InitializeDbProvider();

            var messagesFromDb = dbProvider.GetLastUserMessages(userId, messageCount);

            Assert.NotNull(messagesFromDb);
            Assert.NotNull(messagesFromDb.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Error, messagesFromDb.Status);
            Assert.Equal(exType, messagesFromDb.Exception.GetType());
            Assert.NotNull(messagesFromDb.Result);
            Assert.Empty(messagesFromDb.Result);
        }

        [Fact]
        public void AddMessages_OneMessage_ReturnSuccessDbActionResultWithAddedBlogMessage()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var message = new BlogMessage(user1.Id, "This is message");

            var resultOfAdding = dbProvider.AddMessages(message);

            Assert.NotNull(resultOfAdding);
            Assert.NotNull(resultOfAdding.Result);
            Assert.Null(resultOfAdding.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Success, resultOfAdding.Status);

            Assert.Equal(message.Id, resultOfAdding.Result.ElementAt(0).Id);
            Assert.Equal(message.UserId, resultOfAdding.Result.ElementAt(0).UserId);
            Assert.Equal(message.Message, resultOfAdding.Result.ElementAt(0).Message);
            Assert.Equal(message.CreationTime, resultOfAdding.Result.ElementAt(0).CreationTime);
        }

        [Fact]
        public void AddMessages_AlreadyExistingMessage_ReturnErrorDbActionResultWithException()
        {
            InitializeDbProvider();
            var user1 = users[0];
            var message = new BlogMessage(user1.Id, "This is message");

            dbProvider.AddMessages(message);
            var resultOfAdding = dbProvider.AddMessages(message);

            Assert.NotNull(resultOfAdding);
            Assert.NotNull(resultOfAdding.Result);
            Assert.NotNull(resultOfAdding.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Error, resultOfAdding.Status);
            Assert.Equal(typeof(AddingOperationException), resultOfAdding.Exception.GetType());
        }

        [Fact]
        public void AddMessage_ManyMessages_ReturnSuccessDbActionReslutWithIEnumerableBlogMessages()
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
            Assert.NotNull(resultOfAdding.Result);
            Assert.NotEmpty(resultOfAdding.Result);
            Assert.Null(resultOfAdding.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Success, resultOfAdding.Status);

            foreach (BlogMessage message in resultOfAdding.Result)
            {
                Assert.True(messages.FirstOrDefault(x => x.Id == message.Id) != null);
            }
        }

        [Fact]
        public void AddMessage_EmptyMessageCollection_ReturnErrorDbActionResultWithException()
        {
            InitializeDbProvider();
            var messages = new List<BlogMessage>();

            var resultOfAdding = dbProvider.AddMessages(messages.ToArray());

            Assert.NotNull(resultOfAdding);
            Assert.NotNull(resultOfAdding.Result);
            Assert.NotNull(resultOfAdding.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Error, resultOfAdding.Status);
            Assert.Equal(typeof(ArgumentException), resultOfAdding.Exception.GetType());
        }

        [Fact]
        public void AddMessage_NullCollection_ReturnErrorDbActionResultWithException()
        {
            InitializeDbProvider();
   
            var resultOfAdding = dbProvider.AddMessages(null);

            Assert.NotNull(resultOfAdding);
            Assert.NotNull(resultOfAdding.Result);
            Assert.NotNull(resultOfAdding.Exception);
            Assert.Equal(DbActionResult<IEnumerable<BlogMessage>>.StatusCode.Error, resultOfAdding.Status);
            Assert.Equal(typeof(ArgumentNullException), resultOfAdding.Exception.GetType());
        }

        [Fact]
        public void RemoveMessage_ExistingMessage_ReturnSuccessDbActionResultWithBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "MESSAGE");

            dbProvider.AddMessages(message);
            var removeResult = dbProvider.RemoveMessage(message.Id.ToString());

            Assert.NotNull(removeResult);
            Assert.NotNull(removeResult.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.Success, removeResult.Status);
            Assert.Null(removeResult.Exception);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.NotFound, dbProvider.GetMessage(message.Id.ToString()).Status);
        }

        [Fact]
        public void RemoveMessage_NotExistingMessage_ReturnNotFoundDbActionResultWithBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "MESSAGE");

            var removeResult = dbProvider.RemoveMessage(message.Id.ToString());

            Assert.NotNull(removeResult);
            Assert.NotNull(removeResult.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.NotFound, removeResult.Status);
            Assert.Null(removeResult.Exception);
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void RemoveMessage_NotValidMessageId_ReturnErrorDbActionResultWithException(string? messageId, Type exType)
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "MESSAGE");

            dbProvider.AddMessages(message);
            var removeResult = dbProvider.RemoveMessage(messageId);

            Assert.NotNull(removeResult);
            Assert.NotNull(removeResult.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.Error, removeResult.Status);
            Assert.NotNull(removeResult.Exception);
            Assert.Equal(exType, removeResult.Exception.GetType());
        }

        [Fact]
        public void UpdateMessage_ExistingId_ReturnSuccessDbActionResultWithUpdatedBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");
            var newMessage = new BlogMessage(user.Id, "new MESSAGE");

            dbProvider.AddMessages(message);
            var updateResult = dbProvider.UpdateMessage(message.Id.ToString(), newMessage);

            Assert.NotNull(updateResult);
            Assert.NotNull(updateResult.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.Success, updateResult.Status);
            Assert.Null(updateResult.Exception);
            Assert.Equal(newMessage.Message, dbProvider.GetMessage(message.Id.ToString()).Result.Message);
        }

        [Fact]
        public void UpdateMessage_NotExistingId_ReturnNotFoundDbActionResultWithOldBlogMessage()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");
            var newMessage = new BlogMessage(user.Id, "new MESSAGE");

            var updateResult = dbProvider.UpdateMessage(message.Id.ToString(), newMessage);

            Assert.NotNull(updateResult);
            Assert.NotNull(updateResult.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.NotFound, updateResult.Status);
            Assert.Null(updateResult.Exception);
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentNullException))]
        public void UpdateMessage_NotValidMessageId_ReturnErrorDbActionResultWithException(string? messageId, Type exType)
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");
            var newMessage = new BlogMessage(user.Id, "new MESSAGE");

            var updateResult = dbProvider.UpdateMessage(messageId, newMessage);

            Assert.NotNull(updateResult);
            Assert.NotNull(updateResult.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.Error, updateResult.Status);
            Assert.NotNull(updateResult.Exception);
            Assert.Equal(exType, updateResult.Exception.GetType());
        }

        [Fact]
        public void UpdateMessage_NullNewMessage_ReturnErrorDbActionResultWithException()
        {
            InitializeDbProvider();
            var user = users[0];
            var message = new BlogMessage(user.Id, "OLD message");

            var updateResult = dbProvider.UpdateMessage(message.Id.ToString(), null);

            Assert.NotNull(updateResult);
            Assert.Null(updateResult.Result);
            Assert.Equal(DbActionResult<BlogMessage>.StatusCode.Error, updateResult.Status);
            Assert.NotNull(updateResult.Exception);
            Assert.Equal(typeof(ArgumentNullException), updateResult.Exception.GetType());
        }
    }
}

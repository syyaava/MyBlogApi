using BlogCore.Blog;
using BlogCore.Db;
using BlogCore.Exceptions;
using BlogCore.Validators;
using BlogTests.Mocks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.ComponentModel.DataAnnotations;

namespace BlogTests.Core
{
    public class BlogTests
    {
        private Blog blog;
        public BlogTests()
        {
            InitializeBlog(new User("TestUser1", "TestUser@gmail.com"));
        }

        private void InitializeBlog(User user)
        {
            var dbProvider = new MockBlogDbProvider();
            List<IValidator> validators = new List<IValidator>()
            {
                new UserValidator(),
                new BlogMessageValidator(),
            };
            var mainValidator = new MainDataValidator(validators.ToArray());
            blog = new Blog(user, dbProvider, mainValidator); //TODO: Нужно реализовать валидаторы.
        }

        [Fact]
        public void GetMessage_ValidIdExistingMessage_ReturnSuccessActionResultWithBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var message = new BlogMessage(user.Id, "My first message!");
            blog.AddMessages(new[] { message });

            var result = blog.GetMessage(message.Id);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.Null(result.Exception);
            Assert.Equal(message.Id, result.Result.Id);
            Assert.Equal(message.Message, result.Result.Message);
            Assert.Equal(message.UserId, result.Result.UserId);
        }

        [Fact]
        public void GetMessage_ValidIdNotExistingMessage_ReturnNotFoundActionResultWithNull()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var message = new BlogMessage(user.Id, "My first message!");

            var result = blog.GetMessage(message.Id);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.NotFound, result.Status);
            Assert.Null(result.Exception);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("ASLKNJdlkjanHJsnjka-090-9dfs0-a885797/*974/748as4dfalknsd", typeof(ValidationException))]
        public void GetMessage_NotValidId_ReturnErrorActionResultWithNullAndException(string? id, Type exception)
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);

            var result = blog.GetMessage(id);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.Error, result.Status);
            Assert.NotNull(result.Exception);
            Assert.Equal(exception, result.Exception.GetType());
        }

        [Fact]
        public void GetAllUserMessages_ExistingMessages_ReturnSuccessActionResultWithIEnumerableBlogMessages()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My first message!"),
                new BlogMessage(user.Id, "My SECOND message!"),
                new BlogMessage(user.Id, "My ThIrD message!")
            };
            blog.AddMessages(messages.ToArray());

            var result = blog.GetAllUserMessages();

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.NotEmpty(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.Null(result.Exception);
            Assert.Equal(messages.Count, result.Result.Count());
            foreach(var message in messages)
            {
                Assert.True(result.Result.FirstOrDefault(x => x.Id == message.Id && x.UserId == message.UserId 
                && x.CreationTime == message.CreationTime && x.Message == message.Message) != null);
            }
            Assert.True(result.Result.Last().CreationTime < result.Result.First().CreationTime);
        }

        [Fact]
        public void GetAllUserMessages_NotExistingMessages_ReturnNotFoundActionResultWithNull()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);

            var result = blog.GetAllUserMessages();

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.NotFound, result.Status);
            Assert.Null(result.Exception);
        }

        [Fact]
        public void GetLastUserMessages_MoreOrEqualExistingMessagesCount_ReturnSuccessActionResultWithIEnumerableBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            var messageCount = 5;
            InitializeBlog(user);
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My first message!"),
                new BlogMessage(user.Id, "My SECOND message!"),
                new BlogMessage(user.Id, "My ThIrD message!"),
                new BlogMessage(user.Id, "My Fourth message!"),
                new BlogMessage(user.Id, "My Fifth message!"),
                new BlogMessage(user.Id, "My Sixth message!"),
                new BlogMessage(user.Id, "My SEVENTH message!"),
            };
            blog.AddMessages(messages.ToArray());
            messages.Reverse();

            var result = blog.GetLastUserMessages(messageCount);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.NotEmpty(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.Equal(messageCount, result.Result.Count());
            for(var i = 0; i < messageCount; i++)
            {
                Assert.True(messages[i].Id == result.Result.ElementAt(i).Id && messages[i].UserId == result.Result.ElementAt(i).UserId
                    && messages[i].Message == result.Result.ElementAt(i).Message && messages[i].CreationTime == result.Result.ElementAt(i).CreationTime);
            }
        }

        [Fact]
        public void GetLastUserMessages_LessExistingMessagesCount_ReturnSuccessActionResultWithIEnumerableBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            var messageCount = 15;
            InitializeBlog(user);
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My first message!"),
                new BlogMessage(user.Id, "My SECOND message!"),
                new BlogMessage(user.Id, "My ThIrD message!"),
                new BlogMessage(user.Id, "My Fourth message!"),
                new BlogMessage(user.Id, "My Fifth message!"),
                new BlogMessage(user.Id, "My Sixth message!"),
                new BlogMessage(user.Id, "My SEVENTH message!"),
            };

            blog.AddMessages(messages.ToArray());
            messages.Reverse();

            var result = blog.GetLastUserMessages(messageCount);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.NotEmpty(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.True(messageCount > result.Result.Count());
            for (var i = 0; i < messages.Count; i++)
            {
                Assert.True(messages[i].Id == result.Result.ElementAt(i).Id && messages[i].UserId == result.Result.ElementAt(i).UserId
                    && messages[i].Message == result.Result.ElementAt(i).Message && messages[i].CreationTime == result.Result.ElementAt(i).CreationTime);
            }
        }

        [Fact]
        public void GetLastUserMessages_NotExistingMessages_ReturnNotFoundActionResultWithNull()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            var messageCount = 15;
            InitializeBlog(user);

            var result = blog.GetLastUserMessages(messageCount);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.NotFound, result.Status);
            Assert.Null(result.Exception);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void GetLastUserMessages_NotValidMessagesCount_ReturnErrorActionResultWithException(int messageCount)
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My first message!"),
                new BlogMessage(user.Id, "My SECOND message!"),
                new BlogMessage(user.Id, "My ThIrD message!"),
                new BlogMessage(user.Id, "My Fourth message!"),
                new BlogMessage(user.Id, "My Fifth message!"),
                new BlogMessage(user.Id, "My Sixth message!"),
                new BlogMessage(user.Id, "My SEVENTH message!"),
            };

            blog.AddMessages(messages.ToArray());
            messages.Reverse();

            var result = blog.GetLastUserMessages(messageCount);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.Error, result.Status);
            Assert.NotNull(result.Exception);
            Assert.Equal(typeof(ArgumentOutOfRangeException), result.Exception.GetType());
        }

        [Fact]
        public void AddMessages_ValidOneMessage_ReturnSuccessActionResultWithIEnumerableBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My first message!"),
                new BlogMessage(user.Id, "My SECOND message!"),
                new BlogMessage(user.Id, "My ThIrD message!"),
                new BlogMessage(user.Id, "My Fourth message!"),
                new BlogMessage(user.Id, "My Fifth message!"),
                new BlogMessage(user.Id, "My Sixth message!"),
                new BlogMessage(user.Id, "My SEVENTH message!"),
            };
            var message = messages[0];

            var result = blog.AddMessages(message);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.Single(result.Result);
            Assert.Equal(message.Id, result.Result.ElementAt(0).Id);
            Assert.Equal(message.UserId, result.Result.ElementAt(0).UserId);
            Assert.Equal(message.CreationTime, result.Result.ElementAt(0).CreationTime);
            Assert.Equal(message.Message, result.Result.ElementAt(0).Message);
        }

        [Fact]
        public void AddMessages_ValidSomeMessages_ReturnSuccessActionResultWithIEnumerableBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My first message!"),
                new BlogMessage(user.Id, "My SECOND message!"),
                new BlogMessage(user.Id, "My ThIrD message!"),
                new BlogMessage(user.Id, "My Fourth message!"),
                new BlogMessage(user.Id, "My Fifth message!"),
                new BlogMessage(user.Id, "My Sixth message!"),
                new BlogMessage(user.Id, "My SEVENTH message!"),
            };

            var result = blog.AddMessages(messages.ToArray());

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.Equal(messages.Count, result.Result.Count());
            for(var i = 0; i < messages.Count; i++)
            {
                Assert.Equal(messages[i].Id, result.Result.ElementAt(i).Id);
                Assert.Equal(messages[i].UserId, result.Result.ElementAt(i).UserId);
                Assert.Equal(messages[i].CreationTime, result.Result.ElementAt(i).CreationTime);
                Assert.Equal(messages[i].Message, result.Result.ElementAt(i).Message);
            }            
        }

        [Fact]
        public void AddMessages_ValidAlreadyExistingMessages_ReturnErrorActionResultWithDuplicateMessageAndException()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var messages = new List<BlogMessage>()
            {
                new BlogMessage(user.Id, "My first message!"),
                new BlogMessage(user.Id, "My SECOND message!"),
                new BlogMessage(user.Id, "My ThIrD message!"),
                new BlogMessage(user.Id, "My Fourth message!"),
                new BlogMessage(user.Id, "My Fifth message!"),
                new BlogMessage(user.Id, "My Sixth message!"),
                new BlogMessage(user.Id, "My SEVENTH message!"),
            };
            blog.AddMessages(messages.ToArray());

            var result = blog.AddMessages(messages.ToArray());

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(StatusCode.Error, result.Status);
            Assert.Single(result.Result);
            Assert.NotNull(result.Exception);
            Assert.Equal(typeof(AlreadyExistException), result.Exception.GetType());
        }

        [Fact]
        public void AddMessage_NullMessage_ReturnErrorActionResultWithException()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);

            var result = blog.AddMessages(null);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.NotNull(result.Exception);
            Assert.Equal(typeof(ArgumentNullException), result.Exception.GetType());
        }

        [Fact]
        public void AddMessage_EmptyMessage_ReturnErrorActionResultWithException()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);

            var result = blog.AddMessages(BlogMessage.GetEmptyBlogMessage());

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.NotNull(result.Exception);
            Assert.Equal(typeof(ArgumentException), result.Exception.GetType());
        }

        [Fact]
        public void RemoveMessage_ValidIdExistingMessage_ReturnSuccessActionResultWithRemovedBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var message = new BlogMessage(user.Id, "My first message!");
            blog.AddMessages(new[] { message });

            var result = blog.RemoveMessage(message.Id);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.Null(result.Exception);
            Assert.Equal(StatusCode.NotFound, blog.GetMessage(message.Id).Status);
        }

        [Fact]
        public void RemoveMessage_ValidIdNotExistingMessage_ReturnSuccessActionResultWithRemovedBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var message = new BlogMessage(user.Id, "My first message!");

            var result = blog.RemoveMessage(message.Id);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.NotFound, result.Status);
            Assert.Null(result.Exception);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ValidationException))]
        [InlineData("      ", typeof(ValidationException))]
        public void RemoveMessage_NotValidId_ReturnErrorActionResultWithNullAndException(string? id, Type exception)
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var message = new BlogMessage(user.Id, "My first message!");
            blog.AddMessages(new[] { message });

            var result = blog.RemoveMessage(id);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.Error, result.Status);
            Assert.NotNull(result.Exception);
            Assert.Equal(exception, result.Exception.GetType());
        }

        [Fact]
        public void UpdateMessage_ValidIdExistingMessageAndValidNewBlogMessage_ReturnSuccessActionResultWithNewBlogMessage()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var oldMessage = new BlogMessage(user.Id, "My first message!");
            var newMessage = new BlogMessage(user.Id, "My NEW MESSAGE!");
            blog.AddMessages(new[] { oldMessage });

            var result = blog.UpdateMessage(oldMessage.Id, newMessage);

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(StatusCode.Success, result.Status);
            Assert.Null(result.Exception);
            Assert.Equal(newMessage.Message, result.Result.Message);
            Assert.Equal(oldMessage.Id, result.Result.Id);
        }

        [Fact]
        public void UpdateMessage_ValidIdNotExistingMessageAndValidNewBlogMessage_ReturnNotFoundActionResultWithNull()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var oldMessage = new BlogMessage(user.Id, "My first message!");
            var newMessage = new BlogMessage(user.Id, "My NEW MESSAGE!");

            var result = blog.UpdateMessage(oldMessage.Id, newMessage);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.NotFound, result.Status);
            Assert.Null(result.Exception);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ValidationException))]
        [InlineData("      ", typeof(ValidationException))]
        public void UpdateMessage_NotValidIdMessageAndValidNewBlogMessage_ReturnErrorActionResultWithNullAndException(string? id, Type exception)
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var newMessage = new BlogMessage(user.Id, "My NEW MESSAGE!");

            var result = blog.UpdateMessage(id, newMessage);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.Error, result.Status);
            Assert.NotNull(result.Exception);
            Assert.Equal(exception, result.Exception.GetType());
        }

        [Fact]
        public void UpdateMessage_ValidIdMessageAndNullNewBlogMessage_ReturnErrorActionResultWithNullAndException()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var oldMessage = new BlogMessage(user.Id, "My first message!");
            var newMessage = new BlogMessage(user.Id, "My NEW MESSAGE!");
            blog.AddMessages(oldMessage);

            var result = blog.UpdateMessage(oldMessage.Id, null);

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.Error, result.Status);
            Assert.NotNull(result.Exception);
            Assert.Equal(typeof(ArgumentNullException), result.Exception.GetType());
        }

        [Fact]
        public void UpdateMessage_ValidIdMessageAndEmptyNewBlogMessage_ReturnErrorActionResultWithNullAndException()
        {
            var user = new User("TestUser1", "TestUser@gmail.com");
            InitializeBlog(user);
            var oldMessage = new BlogMessage(user.Id, "My first message!");
            var newMessage = new BlogMessage(user.Id, "My NEW MESSAGE!");
            blog.AddMessages(oldMessage);

            var result = blog.UpdateMessage(oldMessage.Id, BlogMessage.GetEmptyBlogMessage());

            Assert.NotNull(result);
            Assert.Null(result.Result);
            Assert.Equal(StatusCode.Error, result.Status);
            Assert.NotNull(result.Exception);
            Assert.Equal(typeof(ValidationException), result.Exception.GetType());
        }
    }
}

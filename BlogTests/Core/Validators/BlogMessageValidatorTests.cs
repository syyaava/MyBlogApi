using BlogCore.Blog;
using BlogCore.Validators;
using System.ComponentModel.DataAnnotations;

namespace BlogTests.Core.Validators
{
    public class BlogMessageValidatorTests
    {
        private readonly IValidator blogMessageValidator;
        private readonly string notValidId = "kJANOJDFnASO:IEJHfg9a3wherfowiauhf4tg9ayhfAOWIWHE4F97QUH";
        private readonly List<BlogMessage> notValidMessages;

        public BlogMessageValidatorTests() 
        {
            blogMessageValidator = new BlogMessageValidator();
            var user = new User("Babababa", "Babababa@gmail.com");
            var minDateTime = DateTime.MinValue;
            var maxDateTime = DateTime.MaxValue;
            var now = DateTime.Now;
            notValidMessages = new List<BlogMessage>()
            {
                new(""),
                new(notValidId),
                new("", "Message"),
                new("", ""),
                new(notValidId, ""),
                new(notValidId, "Message"),
                new("", "", "", minDateTime, minDateTime),
                new("", "", "", minDateTime, maxDateTime),
                new(notValidId, "", "", minDateTime, minDateTime),
                new("", notValidId, "", minDateTime, minDateTime),
                new(user.Id),
                new(user.Id, ""),
                new("", user.Id, "", minDateTime, minDateTime),
                new(notValidId, user.Id, "", minDateTime, minDateTime),
                new("", "", "Message", minDateTime, minDateTime),
                new("", user.Id, "Message", minDateTime, minDateTime),
                new(notValidId, notValidId, "Message", minDateTime, minDateTime),
                new(notValidId, notValidId, "", minDateTime, minDateTime),
                new(Guid.NewGuid().ToString(), notValidId, "", minDateTime, minDateTime),
                new(Guid.NewGuid().ToString(), "", "Message", minDateTime, minDateTime),
                new(Guid.NewGuid().ToString(), notValidId, "Message", minDateTime, minDateTime),
                new(Guid.NewGuid().ToString(), user.Id, "Message", minDateTime, minDateTime),
                new(Guid.NewGuid().ToString(), user.Id, "Message", now, minDateTime),
                new(Guid.NewGuid().ToString(), "", "", minDateTime, minDateTime),
                new("", "", "", maxDateTime, maxDateTime),
                new(notValidId, "", "", maxDateTime, maxDateTime),
                new("", notValidId, "", maxDateTime, maxDateTime),
                new("", "", "Message", maxDateTime, maxDateTime),
                new(notValidId, notValidId, "Message", maxDateTime, maxDateTime),
                new(notValidId, notValidId, "", maxDateTime, maxDateTime),
                new(Guid.NewGuid().ToString(), notValidId, "", maxDateTime, maxDateTime),
                new(Guid.NewGuid().ToString(), "", "Message", maxDateTime, minDateTime),
                new(Guid.NewGuid().ToString(), notValidId, "Message", maxDateTime, maxDateTime),
                new(Guid.NewGuid().ToString(), user.Id, "Message", maxDateTime, maxDateTime),
                new(Guid.NewGuid().ToString(), user.Id, "Message", now, minDateTime),
                new(Guid.NewGuid().ToString(), "", "", maxDateTime, maxDateTime),
            };
        }

        [Fact]
        public void IsValid_ValidMessage_ReturnTrue()
        {
            var user = new User("Barbara", "barbara@mail.ru");
            var message = new BlogMessage(user.Id, "My first message");

            var result = blogMessageValidator.IsValid(message);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_NotValidBlogMessage_ThrowValidationException()
        {
            foreach (var message in notValidMessages)
            {
                Assert.Throws<ValidationException>(() => blogMessageValidator.IsValid(message));
            }
        }

        [Fact]
        public void IsValid_NotBlogMessage_ThrowArgumeneException()
        {
            var notMessage = new BlogMessageValidator();

            Assert.Throws<ArgumentException>(() => blogMessageValidator.IsValid(notMessage));
        }
    }
}

using BlogCore.Blog;
using BlogCore.Validators;
using System.ComponentModel.DataAnnotations;

namespace BlogTests.Core.Validators
{
    public class UserValidatorTests
    {
        private IValidator userValidator;
        private string notValidId = "kJANOJDFnASO:IEJHfg9a3wherfowiauhf4tg9ayhfAOWIWHE4F97QUH";
        private string notValidUsername = new string('a',256);
        private readonly List<User> notValidUsers;

        public UserValidatorTests()
        {
            userValidator = new UserValidator();            
            notValidUsers = new List<User>()
            {
                new ("", ""),
                new (notValidUsername, ""),
                new ("", "varvar@mail.ru"),
                new (notValidUsername, "varvar@mail.ru"),
                new ("", "varvar@mail"),
                new ("", "varvar"),
                new (notValidUsername, "varvar"),
                new ("varvar",""),
                new ("va",""),
                new ("_$@va",""),
                new (Guid.NewGuid().ToString(), "", ""),
                new (notValidId, "", ""),
                new (Guid.NewGuid().ToString(), "", "varvar@mail.ru"),
                new (notValidId, "", "varvar@mail.ru"),
                new (Guid.NewGuid().ToString(), "", "varvar@mail"),
                new (notValidId, "", "varvar@mail"),
                new (Guid.NewGuid().ToString(), "", "varvar"),
                new (notValidId, "", "varvar"),
                new (Guid.NewGuid().ToString(), "varvar", ""),
                new (notValidId, "varvar", ""),
                new (Guid.NewGuid().ToString(), "va", ""),
                new (notValidId, "va", ""),
                new (Guid.NewGuid().ToString(), "_$@va", ""),
                new (notValidId, "_$@va", "")
            };
        }

        [Fact]
        public void IsValid_ValidUser_ReturnTrue()
        {
            var user = new User("Barbara", "barbara@mail.ru");

            var result = userValidator.IsValid(user);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_NotValidUser_ThrowValidationException()
        {
            foreach(var user in notValidUsers)
            {
                Assert.Throws<ValidationException>(() => userValidator.IsValid(user));
            }
        }

        [Fact]
        public void IsValid_NotUser_ThrowArgumeneException()
        {
            var notUser = new UserValidator();

            Assert.Throws<ArgumentException>(() => userValidator.IsValid(notUser));
        }
    }
}

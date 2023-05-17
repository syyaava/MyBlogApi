using BlogCore.Blog;
using BlogCore.Exceptions;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BlogCore.Validators
{
    public class UserValidator : IValidator
    {
        public Type TypeForValidating => typeof(User);

        public bool IsValid(object value)
        {
            if (value is User user)
            {
                if(string.IsNullOrEmpty(user.Name)) throw new ArgumentNullException(nameof(user.Name));
                if(string.IsNullOrEmpty(user.Id)) throw new ArgumentNullException(nameof(user.Name));
                if(string.IsNullOrEmpty(user.Email)) throw new ArgumentNullException(nameof(user.Name));

                if (!Regex.IsMatch(user.Id, RegexPatterns.UserId)) throw new NotValidValueException("The Id is not correct.");
                if (!Regex.IsMatch(user.Name, RegexPatterns.UserName)) throw new NotValidValueException("The Name is not correct. The name field must contains only letters and/or numbers (4-64 symbols).");
                if (!Regex.IsMatch(user.Email, RegexPatterns.Email)) throw new NotValidValueException("The Email is not correct.");
                return true;
            }
            else
                throw new ArgumentException($"The value is not an object of type \"{TypeForValidating}\".");
        }
    }
}

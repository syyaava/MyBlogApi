using BlogCore.Blog;
using BlogCore.Exceptions;
using System.ComponentModel.DataAnnotations;
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
                //if (string.IsNullOrEmpty(user.Id)) throw new ValidationException();
                //if (string.IsNullOrEmpty(user.Name)) throw new ValidationException();
                //if (string.IsNullOrEmpty(user.Email)) throw new ValidationException();

                if (!CheckRegexMatches(user.Id, RegexPatterns.USER_ID)) 
                    throw new ValidationException($"{nameof(user.Id)} not valid.");
                if (!CheckRegexMatches(user.Email, RegexPatterns.EMAIL)) 
                    throw new ValidationException($"{nameof(user.Email)} not valid.");
                if (!CheckRegexMatches(user.Name, RegexPatterns.USERNAME)) 
                    throw new ValidationException($"{nameof(user.Name)} not valid. The name field must contains only letters and/or numbers (4-64 symbols).");

                return true;
            }
            else
                throw new ArgumentException($"The value is not an object of type \"{TypeForValidating}\".");
        }

        private bool CheckRegexMatches(string input, string pattern) => Regex.IsMatch(input, pattern);
    }
}

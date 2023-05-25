using BlogCore.Blog;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlogCore.Validators
{
    public class BlogMessageValidator : IValidator
    {
        public Type TypeForValidating => typeof(BlogMessage);
        private readonly int minYear = 2000;

        public bool IsValid(object value)
        {
            if (value is BlogMessage blogMessage)
            {
                CheckCreationTime(blogMessage);
                CheckUpdateTime(blogMessage);
                CheckMessage(blogMessage);
                CheckUserId(blogMessage);
                CheckId(blogMessage);
                return true;
            }
            else
                throw new ArgumentException($"The value is not an object of type \"{TypeForValidating}\".");
        }

        private void CheckId(BlogMessage blogMessage)
        {
            if (!CheckRegexMatches(blogMessage.Id, RegexPatterns.BLOGMESSAGE_ID))
                throw new ValidationException($"Message id {blogMessage.Id} is not valid.");
        }

        private void CheckUserId(BlogMessage blogMessage)
        {
            if (!CheckRegexMatches(blogMessage.UserId, RegexPatterns.USER_ID))
                throw new ValidationException($"User id {nameof(blogMessage.UserId)} is not valid.");
        }

        private static void CheckMessage(BlogMessage blogMessage)
        {
            if (string.IsNullOrEmpty(blogMessage.Message))
                throw new ValidationException($"Message is not valid. Message cannot be null or empty");
        }

        private void CheckUpdateTime(BlogMessage blogMessage)
        {
            if (blogMessage.UpdateTime.Year < minYear || blogMessage.UpdateTime.Date > DateTime.Now.Date)
                throw new ValidationException("Update date is future.");
            if (blogMessage.UpdateTime < blogMessage.CreationTime)
                throw new ValidationException("The update time is less than creation time.");
        }

        private void CheckCreationTime(BlogMessage blogMessage)
        {
            if (blogMessage.CreationTime.Year < minYear)
                throw new ValidationException("Date is too old."); //TODO: Немного не нравится формулировка.
            if (blogMessage.CreationTime.Date > DateTime.Now.Date)
                throw new ValidationException("Creation date is future.");
        }

        private bool CheckRegexMatches(string input, string pattern) => Regex.IsMatch(input, pattern);
    }
}

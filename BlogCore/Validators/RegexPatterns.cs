using System.Text.RegularExpressions;

namespace BlogCore.Validators
{
    public static class RegexPatterns
    {
        public const string USER_ID = @"^(user-(\w+)(-(\w+))*)";
        public const string BLOGMESSAGE_ID = @"^((\w+)(-(\w+))*)";
        public const string USERNAME = @"^[a-zA-Z0-9''-'\s]{4,64}$";
        public const string EMAIL = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"; //Спасибо Майкрософт за это.
    }
}

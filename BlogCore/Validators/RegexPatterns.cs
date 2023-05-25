using System.Text.RegularExpressions;

namespace BlogCore.Validators
{
    public static class RegexPatterns
    {
        public const string USER_ID = @"(?im)^user-[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";
        public const string BLOGMESSAGE_ID = @"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$";
        public const string USERNAME = @"^[a-zA-Z0-9''-'\s]{4,64}$";
        public const string EMAIL = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"; //Спасибо Майкрософт за это.
    }
}

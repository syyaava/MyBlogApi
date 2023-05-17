namespace BlogCore.Validators
{
    public static class RegexPatterns
    {
        public static readonly string UserId = @"^user-(\w+)(-(\w+))*";
        public static readonly string UserName = @"(\w|_{4-64})";
        public static readonly string Email = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"; //Спасибо Майкрософт за это.
    }
}

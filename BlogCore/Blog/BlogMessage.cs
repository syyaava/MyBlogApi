namespace BlogCore.Blog
{
    public record BlogMessage(string Id, string UserId, string Message, DateTime CreationTime, DateTime UpdateTime)
    {
        //public const string EMPTY_MESSAGE_FILLER = "**No message**";
        /// <summary>
        /// Create empty message from certain user.
        /// </summary>
        /// <param name="userId"></param>
        public BlogMessage(string userId) : this(userId, "") { }
        /// <summary>
        /// Create new message from certain user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        public BlogMessage(string userId, string message) : this(Guid.NewGuid().ToString(), userId, message, DateTime.Now, DateTime.Now) { }

        public static BlogMessage GetEmptyBlogMessage()
        {
            return new BlogMessage(Guid.Empty.ToString(), User.UNKNOW_USER_ID, "", DateTime.MinValue, DateTime.MinValue);
        }
    }
}

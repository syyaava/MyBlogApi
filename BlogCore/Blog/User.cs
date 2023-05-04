namespace BlogCore.Blog
{
    public record User(string Id, string Name, string Email)
    {
        public User(string name, string email) : this("user-" + Guid.NewGuid(), name, email) { }
    }
}

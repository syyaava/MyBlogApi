using BlogCore.Validators;
using System.Text.RegularExpressions;

namespace BlogCore.Blog
{
    public record User
    {
        public const string UNKNOW_USER_ID = "UNKNOW_USER_ID";
        public readonly string Id; 
        public readonly string Name; 
        public readonly string Email;

        public User(string name, string email) : this("user-" + Guid.NewGuid(), name, email) { }

        public User(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}

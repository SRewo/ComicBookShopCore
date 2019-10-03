using System;

namespace ComicBookShopCore.Services.User
{
    public class UserBasicDto
    {
        public Guid Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserLoginDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class UserTokenDto
    {
        public Guid Id { get; private set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }
}
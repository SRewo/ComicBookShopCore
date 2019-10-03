using System;

namespace ComicBookShopCore.Services.User
{
    public class UserBasicDto
    {
        public string Id { get; private set; }
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
        public string Id { get; private set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }

    public class UserRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
        public UserAddressDto Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserAddressDto
    {
        public string StreetName { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using AutoMapper;
using ComicBookShopCore.Data;
using ComicBookShopCore.Services.User;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace ComicBookShopCore.Services.Tests.User
{
    public class UserServiceTests
    {
       
        private UserRegisterDto GetValidUserRegisterDto()
        {
            var validAddress = new UserAddressDto(){City = "Test", Country = "Poland", PostalCode = "43-200", Region = "Sląskie",StreetName = "Test Street 21"};
            var userDto = new UserRegisterDto() {UserName = "Admin",Password = "123", ConfirmPassword = "123", Email = "mail@mail.com", FirstName = "test", LastName = "adam", Address = validAddress};
            return userDto;
        }

        private Data.User GetValidUser()
        {
            var validAddress = new Address{City = "Test", Country = "Poland", PostalCode = "43-200", Region = "Sląskie",StreetName = "Test Street 21"};
            var user = new Data.User() {UserName = "Admin", Email = "mail@mail.com", FirstName = "test", LastName = "adam", Address = validAddress};
            return user;
        }

        private UserUpdateDto GetValidUserUpdateDto()
        {
            var validAddress = new UserAddressDto(){City = "Test", Country = "Poland", PostalCode = "43-200", Region = "Sląskie",StreetName = "Test Street 21"};
            var userDto = new UserUpdateDto {Email = "mail@mail.com", FirstName = "test", LastName = "adam", Address = validAddress};
            return userDto;
        }

        private Mock<UserManager<Data.User>> GetUserManagerMock()
        {
            var userStore = new Mock<IUserStore<Data.User>>();
            return new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task Login_ValidCall()
        {
            var managerMock = GetUserManagerMock();
            var user = new Data.User() {UserName = "user", Id = new Guid().ToString()};
            managerMock.Setup(x => x.FindByNameAsync("user"))
                .ReturnsAsync(user);
            managerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new [] {"Admin"});
            managerMock.Setup(x => x.CheckPasswordAsync(user, "123")).ReturnsAsync(true);
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile()));
            var service = new UserService(managerMock.Object, mapperConfig.CreateMapper());

            var result = await service.Login("user", "123");

            Assert.IsType<UserTokenDto>(result);
            Assert.Equal(user.UserName, result.Login);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal("Admin", result.Role);
        }

        [Fact]
        public async Task Login_InvalidUsername_ReturnsNull()
        {
            var managerMock = GetUserManagerMock();
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile()));
            var service = new UserService(managerMock.Object, mapperConfig.CreateMapper());

            var result = await service.Login("user", "123");

            Assert.Null(result);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsNull()
        {
            var managerMock = GetUserManagerMock();
            var user = new Data.User();
            managerMock.Setup(x => x.FindByNameAsync("user")).ReturnsAsync(user);
            managerMock.Setup(x => x.CheckPasswordAsync(user, "123")).ReturnsAsync(false);
            var mapper = new Mock<IMapper>();
            var service = new UserService(managerMock.Object, mapper.Object);

            var result = await service.Login("user", "123");

            Assert.Null(result);
        }

        [Fact]
        public async Task Register_ValidCall()
        {
            var managerMock = GetUserManagerMock();
            var userDto = GetValidUserRegisterDto();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            managerMock.Setup(x => x.CreateAsync(It.IsAny<Data.User>(), userDto.Password)).ReturnsAsync(IdentityResult.Success);
            var service = new UserService(managerMock.Object, mapper);

            await service.Register(userDto);

            managerMock.Verify(x => x.CreateAsync(It.IsAny<Data.User>(), userDto.Password), Times.Once);
        }

        [Fact]
        public async Task Register_PasswordDontMatchConfirmPassword_ThrowsValidationException()
        {
            var managerMock = GetUserManagerMock();
            var userDto = new UserRegisterDto() {Password = "123", ConfirmPassword = "321"};
            var mapper = new Mock<IMapper>();
            var service = new UserService(managerMock.Object, mapper.Object );

           var result = await service.Register(userDto);
           Assert.Equal("The password and confirmation password do not match.", result.First().Value);
           Assert.Equal("Password", result.First().Key);
        }

        [Fact]
        public async Task Register_InvalidUserSingleError_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var userDto = GetValidUserRegisterDto(); 
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var errors = new [] {new IdentityError{Code = "Password",Description = "Password is too short."}};
            managerMock.Setup(x => x.CreateAsync(It.IsAny<Data.User>(), userDto.Password))
                .ReturnsAsync(IdentityResult.Failed(errors));
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.Register(userDto);

            Assert.Single(result);
            Assert.Equal(errors.First().Description, result.Values.First());
            Assert.Equal(errors.First().Code, result.Keys.First());
        }

        [Fact]
        public async Task Register_InvalidUserMultipleErrors_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var userDto = GetValidUserRegisterDto();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var errors = new [] {new IdentityError{Code = "Password", Description = "Password is too short."}, new IdentityError {Code = "UserName", Description = "UserName is already taken."} };
            managerMock.Setup(x => x.CreateAsync(It.IsAny<Data.User>(), userDto.Password))
                .ReturnsAsync(IdentityResult.Failed(errors));
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.Register(userDto);

            Assert.Equal(2, result.Count);
            Assert.Equal(errors[0].Code, result.First().Key);
            Assert.Equal(errors[1].Description, result[errors[1].Code]);
        }

        [Fact]
        public async Task Register_InvalidUser_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var userDto = GetValidUserRegisterDto();
            userDto.FirstName = string.Empty;
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.Register(userDto);

            Assert.Single(result);
            Assert.Equal("FirstName", result.First().Key);
            Assert.Equal("The First Name field is required.", result.First().Value);
        }

        [Fact]
        public async Task Register_InvalidAddress_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var userDto = GetValidUserRegisterDto();
            userDto.Address.City = string.Empty;
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.Register(userDto);

            Assert.Single(result);
            Assert.Equal("City", result.First().Key);
            Assert.Equal("The City field is required.", result.First().Value);
        }

        [Fact]
        public async Task Register_AddressIsNull_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var userDto = GetValidUserRegisterDto();
            userDto.Address = null;
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.Register(userDto);

            Assert.Single(result);
            Assert.Equal("Address", result.First().Key);
            Assert.Equal("Address cannot be null", result.First().Value);
        }

        [Fact]
        public async Task UserList_ValidCall()
        {
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock = new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var users = new List<Data.User>(){new Data.User(){UserName = "Admin", FirstName = "Adam", Address = new Address()}, new Data.User(){UserName = "Test123", FirstName = "Marek" ,LastName = "Testowy", Address = new Address()}};
            managerMock.Setup(x => x.GetUsersInRoleAsync("User")).ReturnsAsync(users);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UserList();

            Assert.NotNull(result);
            managerMock.Verify(x => x.GetUsersInRoleAsync("User"), Times.Once);
            Assert.Equal(2, result.Count());
            Assert.Equal("Admin", result.First().UserName);
            Assert.Equal("Marek Testowy", result.Last().Name);
        }

        [Fact]
        public async Task FindUserById_ValidCall()
        {
            var managerMock = GetUserManagerMock();
            var user = new Data.User {UserName = "Admin", Email = "admin@op.pl"};
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.FindUserById("Admin");

            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
            Assert.Equal("Admin", result.UserName);
            Assert.Equal("admin@op.pl", result.Email);
        }

        [Fact]
        public async Task FindUserById_UserNotFound_ReturnsNull()
        {
            var managerMock = GetUserManagerMock();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.FindUserById("Admin");

            managerMock.Verify(x => x.FindByIdAsync("Admin"), Times.Once);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("     ")]
        public async Task FindUserById_ProvidedIdIsNullOrEmpty_ReturnsNull(string id)
        {
            var managerMock = GetUserManagerMock();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.FindUserById(id);

            managerMock.Verify(x => x.FindByIdAsync(id), Times.Never);
            Assert.Null(result);
        }

        [Fact]
        public async Task FindUserByUserName_ValidCall()
        {
            var managerMock = GetUserManagerMock();
            var user = new Data.User {UserName = "Admin", FirstName = "Adam", LastName = "Testowy"};
            managerMock.Setup(x => x.FindByNameAsync("Admin")).ReturnsAsync(user);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.FindUserByUserName("Admin");

            managerMock.Verify(x => x.FindByNameAsync("Admin"), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Admin", result.UserName);
            Assert.Equal("Adam Testowy", result.Name);
        }

        [Fact]
        public async Task FindUserByUserName_UserNotFound_ReturnsNull()
        {
            var managerMock = GetUserManagerMock();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.FindUserByUserName("Admin");

            managerMock.Verify(x => x.FindByNameAsync("Admin"), Times.Once);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task FindUserByUserName_UserNameIsNullOrEmpty_ReturnsNull(string userName)
        {
            var managerMock = GetUserManagerMock();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.FindUserByUserName(userName);

            managerMock.Verify(x => x.FindByNameAsync(userName), Times.Never);
            Assert.Null(result);
        }

         [Fact]
        public async Task UserForUpdate_ValidCall()
        {
            var managerMock = GetUserManagerMock();
            var user = new Data.User {FirstName = "Adam"};
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UserForUpdate("Admin");

            managerMock.Verify(x => x.FindByIdAsync("Admin"), Times.Once);
            Assert.NotNull(result);
            Assert.Equal("Adam", result.FirstName);
        }

        [Fact]
        public async Task UserForUpdate_UserNotFound_ReturnsNull()
        {
            var managerMock = GetUserManagerMock();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UserForUpdate("Admin");

            managerMock.Verify(x => x.FindByIdAsync("Admin"), Times.Once);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task UserForUpdate_IdIsNullOrEmpty_ReturnsNull(string id)
        {
            var managerMock = GetUserManagerMock();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UserForUpdate(id);

            managerMock.Verify(x => x.FindByIdAsync(id), Times.Never);
            Assert.Null(result);
        }
    
        [Fact]
        public async Task UpdateUserInfo_ValidCall()
        {
            var managerMock = GetUserManagerMock();
            var user = GetValidUser();
            var userUpdateDto = GetValidUserUpdateDto();
            userUpdateDto.FirstName = "John";
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            managerMock.Setup(x => x.UpdateAsync(It.IsAny<Data.User>())).ReturnsAsync(IdentityResult.Success);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UpdateUserInfo("Admin", userUpdateDto);

            managerMock.Verify(x => x.UpdateAsync(user), Times.Once);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Admin", user.UserName);
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserInfo_UserNotFound_ReturnsProperError()
        {
            var managerMock = GetUserManagerMock();
            var userUpdateDto = GetValidUserUpdateDto();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UpdateUserInfo("Admin", userUpdateDto);

            managerMock.Verify(x => x.FindByIdAsync("Admin"), Times.Once);
            managerMock.Verify(x => x.UpdateAsync(It.IsAny<Data.User>()), Times.Never);
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Id", result.First().Key);
            Assert.Equal("User with provided id was not found.", result.First().Value);
        }

        [Fact]
        public async Task UpdateUserInfo_InvalidUser_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var user = GetValidUser();
            var userUpdateDto = GetValidUserUpdateDto();
            userUpdateDto.FirstName = "";
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UpdateUserInfo("Admin", userUpdateDto);

            managerMock.Verify(x => x.UpdateAsync(It.IsAny<Data.User>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("FirstName", result.First().Key);
        }

        [Fact]
        public async Task UpdateUserInfo_InvalidAddress_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var user = GetValidUser();
            var userUpdateDto = GetValidUserUpdateDto();
            userUpdateDto.Address.Country = "";
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UpdateUserInfo("Admin", userUpdateDto);

            managerMock.Verify(x => x.UpdateAsync(It.IsAny<Data.User>()), Times.Never);
            Assert.Single(result);
            Assert.Equal("Country", result.First().Key);
        }

        [Fact]
        public async Task UpdateUserInfo_InvalidAddressAndUser_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var user = GetValidUser();
            var userUpdateDto = GetValidUserUpdateDto();
            userUpdateDto.FirstName = "";
            userUpdateDto.Address.City = "";
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UpdateUserInfo("Admin", userUpdateDto);

            managerMock.Verify(x => x.UpdateAsync(It.IsAny<Data.User>()), Times.Never);
            Assert.Equal(2, result.Count);
            Assert.Equal("FirstName", result.First().Key);
            Assert.Equal("City", result.Last().Key);
        }

        [Fact]
        public async Task UpdateUserInfo_UpdateReturnsErrors_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var user = GetValidUser();
            var userUpdateDto = GetValidUserUpdateDto();
            var errors = new [] {new IdentityError(){Code = "Email", Description = "Invalid email"}};
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            managerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed(errors));
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.UpdateUserInfo("Admin", userUpdateDto);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Email", result.First().Key);
        }

        [Fact]
        public async Task UpdatePassword_ValidCall()
        {
            var managerMock = GetUserManagerMock();
            var user = GetValidUser();
            var passwordDto = new ChangePasswordDto{OldPassword = "123", NewPassword = "321", NewPasswordConfirm = "321"};
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            managerMock.Setup(x => x.ChangePasswordAsync(user, "123", "321")).ReturnsAsync(IdentityResult.Success);
            var mapper = new Mock<IMapper>();
            var service = new UserService(managerMock.Object, mapper.Object);

            var result = await service.UpdatePassword("Admin", passwordDto);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdatePassword_PasswordsDontMatch_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var passwordDto = new ChangePasswordDto{OldPassword = "123", NewPassword = "321", NewPasswordConfirm = "32"};
            var mapper = new Mock<IMapper>();
            var service = new UserService(managerMock.Object, mapper.Object);

            var result = await service.UpdatePassword("Admin", passwordDto);

            Assert.Single(result);
            Assert.Equal("Password", result.First().Key);
        }

        [Fact]
        public async Task UpdatePassword_UserNotFound_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var passwordDto = new ChangePasswordDto{OldPassword = "123", NewPassword = "321", NewPasswordConfirm = "321"};
            var mapper = new Mock<IMapper>();
            var service = new UserService(managerMock.Object, mapper.Object);

            var result = await service.UpdatePassword("Admin", passwordDto);

            Assert.Single(result);
            Assert.Equal("Id", result.First().Key);
        }

        [Fact]
        public async Task UpdatePassword_ChangePasswordReturnsErrors_ReturnsProperDictionary()
        {
            var managerMock = GetUserManagerMock();
            var user = GetValidUser();
            var passwordDto = new ChangePasswordDto{OldPassword = "123", NewPassword = "321", NewPasswordConfirm = "321"};
            managerMock.Setup(x => x.FindByIdAsync("Admin")).ReturnsAsync(user);
            var errors = new[]
                {new IdentityError {Code = "Password", Description = "Provided password is too short"}};
            managerMock.Setup(x => x.ChangePasswordAsync(user, "123", "321")).ReturnsAsync(IdentityResult.Failed(errors));
            var mapper = new Mock<IMapper>();
            var service = new UserService(managerMock.Object, mapper.Object);

            var result = await service.UpdatePassword("Admin", passwordDto);

            Assert.Single(result);
            Assert.Equal("Password", result.First().Key);
        }
    }
}
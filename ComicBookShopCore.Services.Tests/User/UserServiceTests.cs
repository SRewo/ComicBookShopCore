using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using AutoMapper;
using ComicBookShopCore.Services.User;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace ComicBookShopCore.Services.Tests.User
{
    public class UserServiceTests
    {
       
        private UserRegisterDto GetValidUser()
        {
            var validAddress = new UserAddressDto(){City = "Test", Country = "Poland", PostalCode = "43-200", Region = "Sląskie",StreetName = "Test Street 21"};
            var userDto = new UserRegisterDto() {UserName = "Admin",Password = "123", ConfirmPassword = "123", Email = "mail@mail.com", FirstName = "test", LastName = "adam", Address = validAddress};
            return userDto;
        }

        [Fact]
        public async Task Login_ValidCall()
        {
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock = new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
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
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile()));
            var service = new UserService(managerMock.Object, mapperConfig.CreateMapper());

            var result = await service.Login("user", "123");

            Assert.Null(result);
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsNull()
        {
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
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
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var userDto = GetValidUser();
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            managerMock.Setup(x => x.CreateAsync(It.IsAny<Data.User>(), userDto.Password)).ReturnsAsync(IdentityResult.Success);
            var service = new UserService(managerMock.Object, mapper);

            await service.Register(userDto);

            managerMock.Verify(x => x.CreateAsync(It.IsAny<Data.User>(), userDto.Password), Times.Once);
        }

        [Fact]
        public async Task Register_PasswordDontMatchConfirmPassword_ThrowsValidationException()
        {
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
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
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var userDto = GetValidUser(); 
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
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var userDto = GetValidUser();
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
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var userDto = GetValidUser();
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
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var userDto = GetValidUser();
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
            var userStore = new Mock<IUserStore<Data.User>>();
            var managerMock =
                new Mock<UserManager<Data.User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var userDto = GetValidUser();
            userDto.Address = null;
            var mapper = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile())).CreateMapper();
            var service = new UserService(managerMock.Object, mapper);

            var result = await service.Register(userDto);

            Assert.Single(result);
            Assert.Equal("Address", result.First().Key);
            Assert.Equal("Address cannot be null", result.First().Value);
        }
    }
}
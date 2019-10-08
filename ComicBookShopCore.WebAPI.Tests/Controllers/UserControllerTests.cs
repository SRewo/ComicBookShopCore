using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ComicBookShopCore.Services.User;
using ComicBookShopCore.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ComicBookShopCore.WebAPI.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetToken_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var userDto = new UserTokenDto(){Id = new Guid().ToString(), Login = "123", Role = "Admin"};
            mock.Mock<IUserService>().Setup(x => x.Login("123", "123")).ReturnsAsync(userDto);
            var controller = mock.Create<UserController>();

            var result = await controller.GetToken(new UserLoginDto() {Login = "123", Password = "123"});

            var requestType = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(requestType.Value);
            Assert.IsType<string>(requestType.Value);
        }

        [Fact]
        public async Task GetToken_LoginMethodReturnsNull_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.GetToken(new UserLoginDto(){Login = "123", Password = "123"});

            var requestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid login/password.", requestResult.Value.ToString());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("123", null)]
        [InlineData(null,"123")]
        [InlineData("", "")]
        [InlineData("Login", "")]
        [InlineData("", "123")]
        public async Task GetToken_LoginOrPasswordIsNullOrEmpty_ReturnsBadRequestObjectResult(string login, string password)
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.GetToken(new UserLoginDto(){Login = login, Password = password});

            var requestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Login/password cannot be null or empty", requestResult.Value.ToString());
        }

        [Fact]
        public async Task RegisterUser_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();
            var userDto = new UserRegisterDto();

            var result = await controller.RegisterUser(userDto);

            Assert.IsType<OkResult>(result);
            mock.Mock<IUserService>().Verify(x => x.Register(userDto, "User"), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_UserDtoIsNull_ReturnsBadRequest()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.RegisterUser(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task RegisterUser_RegisterReturnsErrors_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
            var userDto = new UserRegisterDto();
            mock.Mock<IUserService>().Setup(x => x.Register(userDto, "User"))
                .ReturnsAsync(new Dictionary<string, string>());
            var controller = mock.Create<UserController>();

            var result = await controller.RegisterUser(userDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task RegisterEmployee_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();
            var userDto = new UserRegisterDto();

            var result = await controller.RegisterEmployee(userDto);

            Assert.IsType<OkResult>(result);
            mock.Mock<IUserService>().Verify(x => x.Register(userDto, "Employee"), Times.Once);
        }

        [Fact]
        public async Task RegisterEmployee_UserDtoIsNull_ReturnsBadRequest()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.RegisterEmployee(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task RegisterEmployee_RegisterReturnsErrors_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
            var userDto = new UserRegisterDto();
            mock.Mock<IUserService>().Setup(x => x.Register(userDto, "Employee"))
                .ReturnsAsync(new Dictionary<string, string>());
            var controller = mock.Create<UserController>();

            var result = await controller.RegisterEmployee(userDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
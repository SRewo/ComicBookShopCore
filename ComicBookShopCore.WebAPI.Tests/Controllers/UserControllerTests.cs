using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ComicBookShopCore.Services.User;
using ComicBookShopCore.WebAPI.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ComicBookShopCore.WebAPI.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetUserList_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var list = new List<UserDto>(){new UserDto(){Name = "Adam Test", UserName = "Admin"}};
            mock.Mock<IUserService>().Setup(x => x.UserList()).ReturnsAsync(list);
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserList();

            var response = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(response.Value);
        }

        [Fact]
        public async Task GetUserList_ListReturnsNull_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IUserService>().Setup(x => x.UserList()).ReturnsAsync((IEnumerable<UserDto>)null);
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserList();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetUserInfo_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var userData = new UserDto(){Email = "random@email.pl", Address = new UserAddressDto(){City = "Krakow", Country = "Poland"}, DateOfBirth = new DateTime(1999,01,01)};
            mock.Mock<IUserService>().Setup(x => x.FindUserById("admin")).ReturnsAsync(userData);
            var controller = mock.Create<UserController>();
            controller.ControllerContext = new ControllerContext {HttpContext = new DefaultHttpContext()};
            var claims = new ClaimsPrincipal();
            claims.AddIdentity(new ClaimsIdentity(new List<Claim>{new Claim(ClaimTypes.PrimarySid, "admin")}));
            controller.ControllerContext.HttpContext.User = claims;

            var result = await controller.GetLoggedUserInfo();

            var resultValue = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(resultValue.Value);
            var loggedUserInfo = Assert.IsType<UserDto>(resultValue.Value);
            Assert.Equal(userData.Email, loggedUserInfo.Email);
            Assert.Equal(userData.DateOfBirth, loggedUserInfo.DateOfBirth);
            Assert.Equal(userData.Address.City, loggedUserInfo.Address.City);
        }

        [Fact]
        public async Task GetUserInfo_UserNotFound_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();
            controller.ControllerContext = new ControllerContext{HttpContext = new DefaultHttpContext()};
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {new Claim(ClaimTypes.PrimarySid, "admin")}));
            controller.ControllerContext.HttpContext.User = user;

            var result = await controller.GetLoggedUserInfo();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetUserInfo_IdIsNull_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();
            controller.ControllerContext = new ControllerContext{HttpContext = new DefaultHttpContext()};

            var result = await controller.GetLoggedUserInfo();

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var user = new UserDto{UserName = "Admin", Id = "id"};
            mock.Mock<IUserService>().Setup(x => x.FindUserById("id")).ReturnsAsync(user);
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserById("id");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var userValue = Assert.IsType<UserDto>(ok.Value);
            Assert.Equal(user.Id, userValue.Id);
            Assert.Equal(user.UserName, userValue.UserName);
        }

        [Fact]
        public async Task GetUserById_IdIsNull_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserById(null);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetUserById_UserNotFound_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserById("id");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetUserByUserName_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var user = new UserDto{UserName = "Admin", Id = "id"};
            mock.Mock<IUserService>().Setup(x => x.FindUserByUserName("Admin")).ReturnsAsync(user);
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserByUserName("Admin");

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var userValue = Assert.IsType<UserDto>(ok.Value);
            Assert.Equal(user.Id, userValue.Id);
            Assert.Equal(user.UserName, userValue.UserName);
        }

        [Fact]
        public async Task GetUserByUserName_UserNameIsNull_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserByUserName(null);

            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetUserByUserName_UserNotFound_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<UserController>();

            var result = await controller.GetUserByUserName("Admin");

            Assert.IsType<NotFoundResult>(result.Result);
        }

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
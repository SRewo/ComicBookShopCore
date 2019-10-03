using System;
using System.ComponentModel.DataAnnotations;
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
    }
}
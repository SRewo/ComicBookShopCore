using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using ComicBookShopCore.EmployeeModule.Interfaces;
using ComicBookShopCore.EmployeeModule.ViewModels;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.EmployeeModule
{
    public class LoginViewModelTests
    {
        [Fact]
        public void CheckDb_CannotOpen_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<User>>().Setup(x => x.CanOpen()).Returns(false);
            var model = mock.Create<LoginViewModel>();
            model.CheckDb();

            Assert.False(model.CanLogIn);
            Assert.Equal("Unable to connect with database. Please restart program and try again.", model.ErrorMessage);
        }

        [Fact]
        public void CheckDd_CanOpen_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IOpenable<User>>().Setup(x => x.CanOpen()).Returns(true);
            var model = mock.Create<LoginViewModel>();
            model.CheckDb();

            Assert.True(model.CanLogIn);
        }

        [Theory]
        [InlineData("Tester001", true)]
        [InlineData("John023", false)]
        [InlineData("Mick.D", true)]
        [InlineData("mick.d", false)]
        public void CheckUserExists_ValidCall(string login, bool expectedResult)
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IOpenable<User>>().Setup(x => x.GetAll()).Returns(UserSample());
            var model = mock.Create<LoginViewModel>();

            Assert.Equal(expectedResult, model.CheckUserExists(login));
        }

        [Theory]
        [InlineData("Tester001", "123", false)]
        [InlineData("Tester001", "321", true)]
        public void SignIn_ValidCall(string login, string password, bool hasError)
        {
            SecureString securePassword = new NetworkCredential(string.Empty, password).SecurePassword;

            using var mock = AutoMock.GetLoose();
            mock.Mock<IOpenable<User>>().Setup(x => x.GetAll()).Returns(UserSample());
            mock.Mock<IOpenable<User>>().Setup(x => x.CanOpen()).Returns(true);
            mock.Mock<IContainPassword>().Setup(x => x.Password).Returns(securePassword);
            var model = mock.Create<LoginViewModel>();

            
            var securePassw = mock.Create<IContainPassword>();
            var hasher = new PasswordHasher<User>();
            model.Username = login;
            Assert.True(model.CheckUserExists(login));
            model.SignInCommand.Execute(securePassw);
            if (hasError)
            {
                Assert.NotEmpty(model.ErrorMessage);
                Assert.Equal("Invalid Username or password!", model.ErrorMessage);
            }
            else
            {
                Assert.Null(model.ErrorMessage);
            }
        }



        private IQueryable<User> UserSample()
        {
            var hasher = new PasswordHasher<User>();
            var list = new List<User>()
            {
                new User()
                {
                    UserName = "Tester001",
                },
                new User()
                {
                    UserName = "Mick.D",
                }
            };
            list[0].PasswordHash = hasher.HashPassword(list[0], "123");
            return list.AsQueryable();
        }
    }
}

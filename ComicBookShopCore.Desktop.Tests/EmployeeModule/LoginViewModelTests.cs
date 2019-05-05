using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.EmployeeModule.Interfaces;
using ComicBookShopCore.EmployeeModule.ViewModels;
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
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.EmployeeModule
{
    public class LoginViewModelTests
    {
        [Fact]
        public void CheckDb_CannotOpen_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Employee>>().Setup(x => x.CanOpen()).Returns(false);
            var model = mock.Create<LoginViewModel>();
            model.CheckDb();

            Assert.False(model.CanLogIn);
            Assert.Equal("Unable to connect with database. Please restart program and try again.", model.ErrorMessage);
        }

        [Fact]
        public void CheckDd_CanOpen_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Employee>>().Setup(x => x.CanOpen()).Returns(true);
            var model = mock.Create<LoginViewModel>();
            model.CheckDb();

            Assert.True(model.CanLogIn);
        }

        [Theory]
        [InlineData("123", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3")]
        [InlineData("PaSSwoRd", "eb83c9c2e88e7d635d30595e8f37c7b5d225f559cb62aae7bdae5f2d64796ab0")]
        [InlineData("S#eCuRePa55WoRd", "81889da4660fc5a25b6aa24ad914d7cd2830a71d66e52bab6be4b32015a05027")]
        public void Encrypt_ReturnsValidValue(string normalPassword, string encryptedPassword)
        {
            var actual = LoginViewModel.Encrypt(normalPassword);

            Assert.Equal(encryptedPassword.ToUpper(), actual);
        }

        [Theory]
        [InlineData("123", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3")]
        [InlineData("PaSSwoRd", "eb83c9c2e88e7d635d30595e8f37c7b5d225f559cb62aae7bdae5f2d64796ab0")]
        [InlineData("S#eCuRePa55WoRd", "81889da4660fc5a25b6aa24ad914d7cd2830a71d66e52bab6be4b32015a05027")]
        public void CheckPassword_ValidCall(string passwordWritten, string encryptedPassword)
        {
            var employee = new Employee()
            {
                Password = encryptedPassword
            };

            SecureString secureString = new NetworkCredential(string.Empty, passwordWritten).SecurePassword;
            var result = LoginViewModel.CheckPasswords(employee, secureString);
            Assert.True(result);
        }

        [Theory]
        [InlineData("Tester001", true)]
        [InlineData("John023", false)]
        [InlineData("Mick.D", true)]
        [InlineData("mick.d", false)]
        public void CheckUserExists_ValidCall(string login, bool expectedResult)
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Employee>>().Setup(x => x.GetAll()).Returns(EmployeeSample());
            var model = mock.Create<LoginViewModel>();

            Assert.Equal(expectedResult, model.CheckUserExists(login));
        }

        [Theory]
        [InlineData("Tester001", "123", false)]
        [InlineData("John023", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", true), ]
        [InlineData("Mick.D", "PaSSwoRd", false)]
        [InlineData("mick.d", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", true)]
        [InlineData("Tester001", "eb83c9c2e88e7d635d30595e8f37c7b5d225f559cb62aae7bdae5f2d64796ab0", true)]
        public void SignIn_ValidCall(string login, string password, bool hasError)
        {
            SecureString securePassword = new NetworkCredential(string.Empty, password).SecurePassword;

            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Employee>>().Setup(x => x.GetAll()).Returns(EmployeeSample());
            mock.Mock<IRepository<Employee>>().Setup(x => x.CanOpen()).Returns(true);
            mock.Mock<IContainPassword>().Setup(x => x.Password).Returns(securePassword);
            var model = mock.Create<LoginViewModel>();

            
            var securePassw = mock.Create<IContainPassword>();

            model.Username = login;
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



        private IQueryable<Employee> EmployeeSample()
        {
            var list = new List<Employee>()
            {
                new Employee()
                {
                    Login = "Tester001",
                    Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3"
                },
                new Employee()
                {
                    Login = "Mick.D",
                    Password = "eb83c9c2e88e7d635d30595e8f37c7b5d225f559cb62aae7bdae5f2d64796ab0"
                }
            };
            return list.AsQueryable();
        }
    }
}

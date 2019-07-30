using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Filters;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.OrderModule.ViewModels;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.OrderModule
{
    public class OrderListViewModelTests
    {
        [Fact]
        public void ResetFormAsync_ValidCall()
        {
            var model = new OrderListViewModel(null, null, null)
            {
                SearchWord = "Xd", DateTo = new DateTime(1999, 01, 01), DateFrom = new DateTime(1999, 01, 01),
                IsEmployeeSelected = false, IsUserSelected = false
            };
            Task.Run((async () =>
            {
                await model.ResetFormAsync();
                Assert.Empty(model.SearchWord);
            }));
        }

        [Fact]
        public void Search_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Order>>().Setup(x => x.GetAll()).Returns(OrderSample);
            mock.Mock<IRoleFilter>().Setup(x => x.IsInRolesAsync(UserSample()[0])).Returns(new Task<bool>((() => true)));
            var model = mock.Create<OrderListViewModel>();
            Task.Run(async () =>
            {
                model.OnNavigatedTo(null);
                model.SearchWord = "John";
                await model.SearchAsync().ConfigureAwait(true);
                Assert.Single(model.ViewList);
            });
        }

        [Fact]
        public void Search_DoubleCall()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Order>>().Setup(x => x.GetAll()).Returns(OrderSample);
            mock.Mock<IRoleFilter>().Setup(x => x.IsInRolesAsync(UserSample()[0])).Returns(new Task<bool>((() => true)));
            var model = mock.Create<OrderListViewModel>();
            Task.Run(async () =>
            {
                model.OnNavigatedTo(null);
                model.SearchWord = "John";
                await model.SearchAsync().ConfigureAwait(true);
                model.DateFrom = new DateTime(2019, 04, 19);
                await model.SearchAsync().ConfigureAwait(true);
                Assert.StrictEqual(2, model.ViewList.Count());
            });
        }

        private IQueryable<Order> OrderSample()
        {
            var orders = new List<Order>()
            {
                new Order()
                {
                    Employee = UserSample()[0],
                    Date = new DateTime(2019,06,20)
                },
                new Order()
                {
                Employee = UserSample()[0],
                Date = new DateTime(2019,04,20)
            }
            };
            return orders.AsEnumerable().AsQueryable();
        }

        private List<User> UserSample()
        {
            var users = new List<User>()
            {
                new User()
                {
                    FirstName = "John",
                    LastName = "Kent"
                },
                new User()
                {
                    FirstName = "Martin",
                    LastName = "Won",
                }
            };
            return users;
        }
    }
}

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
using MockQueryable.Moq;
using Prism.Regions;
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
        public async Task Search_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var orderListMock = TestData.OrderSample();
            mock.Mock<IRepository<Order>>().Setup(x => x.GetAll()).Returns(orderListMock);
            var taskTrue = Task.FromResult(true);
            var taskFalse = Task.FromResult(false);
            mock.Mock<IRoleFilter>().Setup(x => x.IsInRolesAsync(TestData.UserSample()[0])).Returns(taskTrue);
            mock.Mock<IRoleFilter>().Setup(x => x.IsInRolesAsync(TestData.UserSample()[1])).Returns(taskFalse);
            var model = mock.Create<OrderListViewModel>();
            await model.GetDataAsync().ConfigureAwait(true);
            await model.ResetFormAsync();
            model.DateFrom = new DateTime(2019,01,01);
	    model.DateTo = new DateTime(2019, 12,31);
            model.SearchWord = "John";
            await model.SearchAsync(mock.Create<IRoleFilter>()).ConfigureAwait(true);
            Assert.Empty(model.ViewList);
        }

    }
}

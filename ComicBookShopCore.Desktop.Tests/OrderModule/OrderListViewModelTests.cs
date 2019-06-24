using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ComicBookShopCore.OrderModule.ViewModels;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.OrderModule
{
    public class OrderListViewModelTests
    {
        [Fact]
        public void ResetFormAsync_ValidCall()
        {
            var model = new OrderListViewModel(null, null) {SearchWord = "Xd", DateTo = new DateTime(1999,01,01),DateFrom = new DateTime(1999,01,01), IsEmployeeSelected = false, IsUserSelected = false};
            Task.Run((async () =>
            {
                await model.ResetFormAsync();
                Assert.Empty(model.SearchWord);
            }));
        }

    }
}

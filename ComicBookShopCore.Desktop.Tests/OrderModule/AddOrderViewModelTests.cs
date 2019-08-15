using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.OrderModule.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ComicBookShopCore.Data.Builders;
using Unity.Events;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.OrderModule
{
    public class AddOrderViewModelTests
    {
        
        [Fact]
        public void GetData_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(TestData.GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.GetData();

            Assert.NotNull(model.ComicBooks);
            Assert.NotNull(model.Publishers);
            Assert.Equal(TestData.GetComicBooksSample().First().Title, model.ComicBooks.First().Title);
            Assert.Equal(TestData.GetPublishersSample().First().Name, model.Publishers.First().Name);
        }

        [Fact]
        public void AddItemCommand_WithoutErrors_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null, new User[1]);
            model.OrderItems = new ObservableCollection<OrderItemInputModel>();
            model.SelectedComicBook = TestData.GetComicBooksSample().First();
            model.AddItemCommand.Execute();

            var actualItem = model.OrderItems.First();

            Assert.NotNull(actualItem);
            Assert.Equal(model.SelectedComicBook, actualItem.ComicBook);
            Assert.Equal(0, actualItem.Discount);
            Assert.Equal(1, actualItem.Quantity);
        }

        [Fact]
        public void AddItemCommand_SelectedComicBookIsNull_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null, new User[1]);
            model.OrderItems = new ObservableCollection<OrderItemInputModel>();
            model.AddItemCommand.Execute();

            Assert.Empty(model.OrderItems);
        }

        [Fact]
        public void AddItemCommand_SelectedComicBookAlreadyInCollection_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null, new User[1]);
            model.OrderItems = new ObservableCollection<OrderItemInputModel>();
            model.SelectedComicBook = TestData.GetComicBooksSample().First();
	    model.AddItemCommand.Execute();
            model.SelectedComicBook = TestData.GetComicBooksSample().First();
            model.AddItemCommand.Execute();

            Assert.Single(model.OrderItems);
        }

        [Fact]
        public void RemoveItemCommand_WithoutErrors_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null, new User[1]);
            model.OrderItems = new ObservableCollection<OrderItemInputModel>();
            model.SelectedComicBook = TestData.GetComicBooksSample().First();
            model.AddItemCommand.Execute();
            model.SelectedOrderItem = model.OrderItems.First();
            model.RemoveItemCommand.Execute();

            Assert.Empty(model.OrderItems);
        }

        [Fact]
        public void RemoveItemCommand_SelectedOrderItemIsNull_ValidCall()
        {
            var model = new AddOrderViewModel(null, null, null, null, new User[1]);
            model.OrderItems = new ObservableCollection<OrderItemInputModel>();
            model.SelectedComicBook = TestData.GetComicBooksSample().First();
            model.AddItemCommand.Execute();
            model.SelectedOrderItem = null;
            model.RemoveItemCommand.Execute();

            Assert.NotEmpty(model.OrderItems);
        }

        [Fact]
        public void SelectedPublisherChangedCommand_WithoutSearchWord_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(TestData.GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.SelectedPublisher = TestData.GetPublishersSample().First();
            model.SearchWord = string.Empty;
            model.GetData();
            model.SelectedPublisherChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Dark Nights Metal: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SelectedPublisherChanged_WithSearchWord_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(TestData.GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.SelectedPublisher = TestData.GetPublishersSample().First();
            model.GetData();
            model.SearchWord = "Dark";
            model.SelectedPublisherChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Dark Nights Metal: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SearchWordChanged_WithoutPublisher_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(TestData.GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.GetData();
            model.SearchWord = "Ant";
            model.SearchWordChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Ant Man Last Days: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SearchWordChanged_WithPublisher_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(TestData.GetComicBooksSample);
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(TestData.GetPublishersSample);
            var model = mock.Create<AddOrderViewModel>();
            model.GetData();
            model.SearchWord = "Ant";
            model.SelectedPublisher = TestData.GetPublishersSample().ToList()[1];
            model.SearchWordChangedCommand.Execute();

            Assert.Single(model.ComicBooks);
            Assert.Equal("Ant Man Last Days: #1", model.ComicBooks.First().Title);
        }

        [Fact]
        public void SaveOrder_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddOrderViewModel>();
            model.OrderItems = new ObservableCollection<OrderItemInputModel>();
            var comicBook = TestData.GetComicBooksSample().First();
            model.SelectedComicBook = comicBook;
            model.AddItemCommand.Execute();
            model.SaveOrderCommand.Execute();

            mock.Mock<IRepository<Order>>().Verify(x => x.Add(model.Order), Times.Once);
            Assert.Equal(TestData.GetComicBooksSample().First().Quantity -1, comicBook.Quantity);
        }

        [Fact]
        public void SaveOrder_QuantityIsOverLimit_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddOrderViewModel>();
            model.OrderItems = new ObservableCollection<OrderItemInputModel>();
            var comicBook = TestData.GetComicBooksSample().First();
            model.SelectedComicBook = comicBook;
            model.AddItemCommand.Execute();
            model.OrderItems.First().Quantity = 211;
            Assert.Equal(1, model.OrderItems.First().Quantity);

        }


    }
}

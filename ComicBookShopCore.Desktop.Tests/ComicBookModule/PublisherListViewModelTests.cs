using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.ComicBookModule.Views;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class PublisherListViewModelTests
    {
        [Fact]
        public void IsSearchEnabled_ViewListIsNull_EqualsFalse()
        {

            var model = new PublishersListViewModel(null, null)
            {
                ViewList = null
            };

            Assert.False(model.IsSearchEnabled);

        }

        [Fact]
        public void IsSearchEnabled_ViewListNotNull_EqualsTrue()
        {

            var model = new PublishersListViewModel(null, null)
            {
                ViewList = new List<Publisher>()
            };
            model.CanSearchCheck();

            Assert.True(model.IsSearchEnabled);

        }

        [Fact]
        public void IsEditEnabled_SelectedPublisherIsNull_EqualsFalse()
        {
            var model = new PublishersListViewModel(null, null);

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedPublisherNotNull_EqualsTrue()
        {
            var model = new PublishersListViewModel(null, null)
            {
                SelectedPublisher = TestData.GetPublishersSample().First() 
            };

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedPublisherChanged_EqualsTrue()
        {

            var model = new PublishersListViewModel(null, null);

            Assert.False(model.IsEditEnabled);

            model.SelectedPublisher = TestData.GetPublishersSample().First(); 

            Assert.True(model.IsEditEnabled);

        }

        [Fact]
        public void GetData_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(TestData.GetPublishersSample);
            var model = mock.Create<PublishersListViewModel>();
            model.GetData();

            var expected = TestData.GetPublishersSample().Count();
            var actual = model.ViewList.Count;

            Assert.NotNull(model.ViewList);
            Assert.Equal(expected, actual);
            mock.Mock<IRepository<Publisher>>().Verify(x => x.GetAll(), Times.Exactly(1));
        }

        [Fact]
        public void OpenAdd_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditPublisher"));
            var model = mock.Create<PublishersListViewModel>();
            model.OpenAdd();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditPublisher"));
        }

        [Fact]
        public void OpenEdit_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var publisher = TestData.GetPublishersSample().First(); 
            var parameters = new NavigationParameters()
                {
                    {"publisher",publisher}
                };

            mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditPublisher", parameters));
            var model = mock.Create<PublishersListViewModel>();
            model.SelectedPublisher = publisher;
            model.OpenEdit();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditPublisher", parameters), Times.Once);
        }

    }
}

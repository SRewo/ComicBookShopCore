using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class SeriesListViewModelTests
    {
        [Fact]
        public void IsEditEnabled_SelectedSeriesIsNull_EqualsFalse()
        {
            var model = new SeriesListViewModel(null,null,null);

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SeriesIsSelected_EqualsTrue()
        {
            var model = new SeriesListViewModel(null, null, null)
            {
                SelectedSeries = TestData.GetSeriesSample().First()
            };

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void OpenAdd_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditSeries"));

            var model = mock.Create<SeriesListViewModel>();

            model.AddSeriesCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditSeries"), Times.Once);
        }

        [Fact]
        public void OpenEdit_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var series = TestData.GetSeriesSample().First();
            var parameters = new NavigationParameters()
                {
                    { "series",series }
                };
            mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditSeries", parameters));
            var model = mock.Create<SeriesListViewModel>();
            model.SelectedSeries = series;
            model.EditSeriesCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditSeries", parameters), Times.Once);
        }

        [Fact]
        public void ResetSearchCommand_ValidCall()
        {
            var model = new SeriesListViewModel(null, null, null)
            {
                SearchWord = "Word",
                SelectedPublisher = TestData.GetPublishersSample().First()
            };
            model.ResetSearchCommand.Execute();

            Assert.Empty(model.SearchWord);
            Assert.Null(model.SelectedPublisher);
        }

        [Fact]
        public void GetPublisherData_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(TestData.GetPublishersSample);
            var model = mock.Create<SeriesListViewModel>();
            model.GetPublisherData();
            var expectedCount = TestData.GetPublishersSample().Count();
            var actualCount = model.Publishers.Count;

            mock.Mock<IRepository<Publisher>>().Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void GetSeriesData_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(TestData.GetSeriesSample);
            var model = mock.Create<SeriesListViewModel>();
            model.GetSeriesData();
            var expectedCount = TestData.GetSeriesSample().Count();
            var actualCount = model.ViewList.Count;

            mock.Mock<IRepository<Series>>().Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public void Search_SelectedPublisherChanged_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(TestData.GetSeriesSample);
            var model = mock.Create<SeriesListViewModel>();
            model.GetSeriesData();
            model.SelectedPublisher = TestData.GetPublishersSample().First();
            model.SearchWord = String.Empty;
            model.Search();
            var expectedCount = 1;
            var expectedNameFirst = "Dark Nights Metal";
            var actualCount = model.ViewList.Count;
            var actualNameFirst = model.ViewList.First().Name;

            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedNameFirst, actualNameFirst);
        }

        [Fact]
        public void Search_SearchWordChanged_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(TestData.GetSeriesSample);
            var model = mock.Create<SeriesListViewModel>();
            model.GetSeriesData();
            model.SearchWord = "Ant";
            model.Search();
            var expectedCount = 1;
            var expectedNameFirst = "Ant-Man: Last Days";
            var actualCount = model.ViewList.Count;
            var actualNameFirst = model.ViewList.First().Name;

            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedNameFirst, actualNameFirst);
        }

        [Fact]
        public void Search_SearchWordAndPublisherChanged_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(TestData.GetSeriesSample);
            var model = mock.Create<SeriesListViewModel>();
            model.GetSeriesData();
            model.SearchWord = "Ant";
            model.SearchWordChanged.Execute();
            model.SelectedPublisher = TestData.GetPublishersSample().ToList()[2];
            model.SelectedPublisherChanged.Execute();
            var expectedCount = 0;
            var actualCount = model.ViewList.Count;
            
            Assert.Equal(expectedCount, actualCount);
        }

    }
}

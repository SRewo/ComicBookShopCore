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

            model.SelectedPublisher = null;

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SeriesIsSelected_EqualsTrue()
        {
            var model = new SeriesListViewModel(null,null,null);

            model.SelectedSeries = new Series();

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void OpenAdd_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditSeries"));

                var model = mock.Create<SeriesListViewModel>();

                model.AddSeriesCommand.Execute();

                mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content","AddEditSeries"), Times.Once);
            }
        }

        [Fact]
        public void OpenEdit_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var series = new Series();
                var parameters = new NavigationParameters()
                {
                    { "series",series }
                };
                mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditSeries", parameters));
                var model = mock.Create<SeriesListViewModel>();
                model.SelectedSeries = series;
                model.EditSeriesCommand.Execute();

                mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content","AddEditSeries",parameters), Times.Once);

            }
        }

        [Fact]
        public void ResetSearchCommand_ValidCall()
        {
            var model = new SeriesListViewModel(null,null,null);
            model.SearchWord = "Word";
            model.SelectedPublisher = new Publisher();
            model.ResetSearchCommand.Execute();

            Assert.Empty(model.SearchWord);
            Assert.Null(model.SelectedPublisher);
        }

        [Fact]
        public void GetPublisherData_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(GetPublishersSample);
                var model = mock.Create<SeriesListViewModel>();
                model.GetPublisherData();
                var expectedCount = GetPublishersSample().Count();
                var actualCount = model.Publishers.Count;

                mock.Mock<IRepository<Publisher>>().Verify(x => x.GetAll(), Times.Once);
                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void GetSeriesData_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(GetSeriesSample);
                var model = mock.Create<SeriesListViewModel>();
                model.GetSeriesData();
                var expectedCount = GetSeriesSample().Count();
                var actualCount = model.ViewList.Count;

                mock.Mock<IRepository<Series>>().Verify(x => x.GetAll(), Times.Once);
                Assert.Equal(expectedCount, actualCount);
            }
        }

        [Fact]
        public void Search_SelectedPublisherChanged_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(GetSeriesSample);
                var model = mock.Create<SeriesListViewModel>();
                model.GetSeriesData();
                model.SelectedPublisher = GetPublishersSample().First();
                model.SearchWord = String.Empty;
                model.Search();
                var expectedCount = 1;
                var expectedNameFirst = "Dark Nights Metal";
                var actualCount = model.ViewList.Count;
                var actualNameFirst = model.ViewList.First().Name;

                Assert.Equal(expectedCount,actualCount);
                Assert.Equal(expectedNameFirst,actualNameFirst);
            }
        }

        [Fact]
        public void Search_SearchWordChanged_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(GetSeriesSample);
                var model = mock.Create<SeriesListViewModel>();
                model.GetSeriesData();
                model.SearchWord = "Ant";
                model.Search();
                var expectedCount = 1;
                var expectedNameFirst = "Ant-Man: Last Days";
                var actualCount = model.ViewList.Count;
                var actualNameFirst = model.ViewList.First().Name;

                Assert.Equal(expectedCount, actualCount);
                Assert.Equal(expectedNameFirst,actualNameFirst);
            }
        }

        [Fact]
        public void Search_SearchWordAndPublisherChanged_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(GetSeriesSample);
                var model = mock.Create<SeriesListViewModel>();
                model.GetSeriesData();
                model.SearchWord = "Ant";
                model.SearchWordChanged.Execute();
                model.SelectedPublisher = GetPublishersSample().ToList()[2];
                model.SelectedPublisherChanged.Execute();
                var expectedCount = 0;
                var actualCount = model.ViewList.Count;

                Assert.Equal(expectedCount, actualCount);
            }
        }

        private IQueryable<Series> GetSeriesSample()
        {
            var publishers = GetPublishersSample().ToList();
            var series = new List<Series>()
            {
                new Series(1)
                {
                    Name = "Dark Nights Metal",
                    Description = "Series one",
                    Publisher = publishers[0]
                },
                new Series(2)
                {
                    Name = "Ant-Man: Last Days",
                    Description = "Ant-Man",
                    Publisher = publishers[1]
                }
            };

            return series.AsEnumerable().AsQueryable();

        }

        private IQueryable<Publisher> GetPublishersSample()
        {
            var tmp = new List<Publisher>()
            {
                new Publisher(1)
                {
                    Name = "DC Comics",
                    CreationDateTime = DateTime.Parse("01.01.1934"),
                    Description = "Some random description."
                },
                new Publisher(2)
                {
                    Name = "Marvel Comics",
                    CreationDateTime = DateTime.Parse("01.01.1939"),
                    Description = "Another description"
                },
                new Publisher(3)
                {
                    Name = "Dark Horse Comics",
                    CreationDateTime = DateTime.Parse("01.01.1986"),
                    Description = " American comic book and manga publisher."
                }
            };

            return tmp.AsEnumerable().AsQueryable();
        }
    }
}

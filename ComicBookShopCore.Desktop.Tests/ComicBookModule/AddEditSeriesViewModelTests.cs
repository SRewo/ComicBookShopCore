using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable.Moq;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class AddEditSeriesViewModelTests
    {
        [Fact]
        public void OnNavigatedTo_ValidCall()
        {
            var mock = new Mock<AddEditSeriesViewModel>(null, null, null);
            var model = mock.Object;

            model.OnNavigatedTo(new NavigationContext(null,null));

            mock.Verify(x => x.ResetFormAsync(), Times.Once);
            mock.Verify(x => x.CheckPassedSeriesAsync(null), Times.Once);
            mock.Verify(x => x.GetPublishersFromRepositoryAsync(), Times.Once);
            mock.Verify(x => x.SetErrorsChangedEventAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPublishersFromRepository_ValidCall()
        {
            var mock = TestData.GetPublishersSample();
            var mockRepository = new Mock<IRepository<Publisher>>();
            mockRepository.Setup(x => x.GetAll()).Returns(mock);

            var model = new AddEditSeriesViewModel(null,mockRepository.Object,null);
            
            await model.GetPublishersFromRepositoryAsync().ConfigureAwait(true);
            Assert.NotNull(model.Publishers);
            Assert.Equal(3, model.Publishers.Count);
        }

        [Fact]
        public void SaveSeriesCommand_Adding_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditSeriesViewModel>();
            var series = TestData.GetSeriesSample().First();
            model.CheckPassedSeriesAsync(null);
            model.SetErrorsChangedEventAsync();

            model.InputModel.Name = series.Name;
            model.InputModel.Description = series.Description;
            model.InputModel.Publisher = series.Publisher;

            model.SaveSeriesCommand.Execute();

            mock.Mock<IRepository<Series>>().Verify(x => x.Add(model.Series), Times.Once);
        }

        [Fact]
        public void SaveSeriesCommand_Updating_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditSeriesViewModel>();
            var series = TestData.GetSeriesSample().First();
            model.CheckPassedSeriesAsync(series);
            model.SetErrorsChangedEventAsync();

            model.InputModel.Name = "New Name";

            model.SaveSeriesCommand.Execute();

            mock.Mock<IRepository<Series>>().Verify(x => x.Update(model.Series), Times.Once);
        }

        [Fact]
        public void GoBackCommand_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditSeriesViewModel>();
            model.GoBackCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "SeriesList"), Times.Once);
        }

        [Fact]
        public void NameErrorMessage_SetsProperly()
        {
            var model = new AddEditSeriesViewModel(null, null, null);
            model.CheckPassedSeriesAsync(null);
            model.SetErrorsChangedEventAsync();
            model.InputModel.Name = " ";

            Assert.True(model.InputModel.HasErrors);
            Assert.False(model.CanSave);
            Assert.Equal(model.InputModel.GetFirstError("Name"), model.NameErrorMessage);
        }

        [Fact]
        public void CanSave_PublisherIsNull_ReturnsFalse()
        {
            var model = new AddEditSeriesViewModel(null, null, null);
            model.CheckPassedSeriesAsync(null);
            model.SetErrorsChangedEventAsync();
            model.InputModel.Name = "Random Name";
            model.InputModel.Description = "Rand desc";

            Assert.False(model.CanSave);

        }

    }
}

using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class AddEditArtistViewModelTests
    {
        [Fact]
        public void OnNavigatedTo_WithoutParameters_ValidCall()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.OnNavigatedTo(new NavigationContext(null, null));

            Assert.NotNull(model.Artist);
        }

        [Fact]
        public void OnNavigatedTo_WithParameter_ValidCall()
        {
            var artist = new Artist(1)
            {
                FirstName = "Scott",
                LastName = "Snyder",
                Description = "Some random descryption"
            };
            var parameters = new NavigationParameters()
            {
                { "Artist", artist }
            };
            var uri = new Uri("test?name=value", UriKind.Relative);
            var model = new AddEditArtistViewModel(null, null);

            using var mock = AutoMock.GetLoose();
            IRegion region = new Region();
            mock.Mock<IRegionNavigationService>().Setup(x => x.Region).Returns(region);
            var navigationService = mock.Create<IRegionNavigationService>();

            var navigationContext = new NavigationContext(navigationService, uri, parameters);
            model.OnNavigatedTo(navigationContext);

            Assert.NotNull(model.Artist);
            Assert.Equal(artist.Id, model.Artist.Id);
            Assert.Equal(artist.Name, model.Artist.Name);
            Assert.Equal(artist.Description, model.Artist.Description);
        }

        [Fact]
        public void GoBackCommand_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditArtistViewModel>();
            model.GoBackCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "ArtistList"), Times.Once);
        }

        [Fact]
        public void SaveArtistCommand_Updating_ValidExecute()
        {
            var artist = new Artist(1)
            {
                FirstName = "Scott",
                LastName = "Snyder",
                Description = "Some random descryption"
            };
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditArtistViewModel>();
            model.Artist = artist;
            model.SaveArtistCommand.Execute();

            mock.Mock<IRepository<Artist>>().Verify(x => x.Update(artist), Times.Once);
            mock.Mock<IRepository<Artist>>().Verify(x => x.Add(artist), Times.Never);
        }

        [Fact]
        public void SaveArtistCommand_Adding_ValidExecute()
        {
            var artist = new Artist()
            {
                FirstName = "Scott",
                LastName = "Snyder",
                Description = "Some random descryption"
            };
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditArtistViewModel>();
            model.Artist = artist;
            model.SaveArtistCommand.Execute();

            mock.Mock<IRepository<Artist>>().Verify(x => x.Update(artist), Times.Never);
            mock.Mock<IRepository<Artist>>().Verify(x => x.Add(artist), Times.Once);
        }

        [Fact]
        public void FirstNameErrorMessage_SetsProperly()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.Artist = new Artist();
            model.Artist.ErrorsChanged += model.Artist_ErrorsChanged;
            model.Artist.FirstName = "";

            var expectedMessage = "First name cannot be empty.";
            var actualMessage = model.FirstNameErrorMessage;

            Assert.NotEmpty(model.FirstNameErrorMessage);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void LastNameErrorMessage_SetsProperly()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.Artist = new Artist();
            model.Artist.ErrorsChanged += model.Artist_ErrorsChanged;
            model.Artist.LastName = "";

            var expectedMessage = "Last name cannot be empty.";
            var actualMessage = model.LastNameErrorMessage;

            Assert.NotEmpty(model.LastNameErrorMessage);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void CanSave_ProperArtist_ReturnsTrue()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.Artist = new Artist();
            model.Artist.PropertyChanged += model.ArtistOnPropertyChanged;
            model.Artist.FirstName = "Scott";
            model.Artist.LastName = "Snyder";

            Assert.True(model.CanSave);
        }

        [Fact]
        public void CanSave_WithoutFirstName_ReturnsFalse()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.Artist = new Artist();
            model.Artist.PropertyChanged += model.ArtistOnPropertyChanged;
            model.Artist.LastName = "Snyder";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanSave_WithoutLastName_ReturnsFalse()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.Artist = new Artist();
            model.Artist.PropertyChanged += model.ArtistOnPropertyChanged;
            model.Artist.FirstName = "Scott";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanSave_WithArtistError_ReturnsFalse()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.Artist = new Artist();
            model.Artist.PropertyChanged += model.ArtistOnPropertyChanged;
            model.Artist.FirstName = "Scott";
            model.Artist.LastName = "!";

            Assert.False(model.CanSave);
        }
    }
}

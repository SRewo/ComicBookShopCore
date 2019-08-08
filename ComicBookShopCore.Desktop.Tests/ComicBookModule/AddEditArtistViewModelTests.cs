using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class AddEditArtistViewModelTests
    {
        [Fact]
        public void OnNavigatedTo_ValidCall()
        {
            var mock = new Mock<AddEditArtistViewModel>(null, null);
            var model = mock.Object;
            model.OnNavigatedTo(new NavigationContext(null, null));
            
            mock.Verify(x => x.ResetFormAsync(),Times.Once);
            mock.Verify(x => x.SetErrorMessageChangesAsync(), Times.Once);
            mock.Verify(x => x.CheckPassedArtistAsync(null), Times.Once());
        }

        [Fact]
        public void CheckPassedArtist_WithArtist_ValidCall()
        {
            var artist = TestData.GetArtistSample().ToList()[1];
            var model = new AddEditArtistViewModel(null, null);

            model.CheckPassedArtistAsync(artist);

            Assert.False(model.CanSave);
            Assert.Equal(artist.FirstName, model.InputModel.FirstName);
            Assert.Equal(artist.LastName, model.InputModel.LastName);
            Assert.Equal(artist.Description, model.InputModel.Description);
        }

        [Fact]
        public void CheckPassedArtist_ArtistIsNull_ValidCall()
        {
            var model = new AddEditArtistViewModel(null, null);

            model.CheckPassedArtistAsync(null);

            Assert.False(model.CanSave);
            Assert.Null(model.InputModel.FirstName);
            Assert.Null(model.InputModel.LastName);
            Assert.Null(model.InputModel.Description);
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
            var artist = TestData.GetArtistSample().ToList()[1];
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditArtistViewModel>();

            model.CheckPassedArtistAsync(artist);
            model.SaveArtistCommand.Execute();
            mock.Mock<IRepository<Artist>>().Verify(x => x.Update(model.Artist), Times.Once);
        }

        [Fact]
        public void SaveArtistCommand_Adding_ValidExecute()
        {
            var artist = TestData.GetArtistSample().ToList()[0];
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditArtistViewModel>();
            model.CheckPassedArtistAsync(null);
            model.InputModel.FirstName = artist.FirstName;
            model.InputModel.LastName = artist.LastName;
            model.InputModel.Description = artist.Description;
            model.SaveArtistCommand.Execute();

            mock.Mock<IRepository<Artist>>().Verify(x => x.Update(model.Artist), Times.Never);
            mock.Mock<IRepository<Artist>>().Verify(x => x.Add(model.Artist), Times.Once);
        }

        [Fact]
        public void FirstNameErrorMessage_SetsProperly()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.SetErrorMessageChangesAsync();
            model.InputModel.FirstName = "";

            var expectedMessage = "First name cannot be empty.";
            var actualMessage = model.FirstNameErrorMessage;

            Assert.NotEmpty(model.FirstNameErrorMessage);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void LastNameErrorMessage_SetsProperly()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.SetErrorMessageChangesAsync();

            model.InputModel.LastName = "";

            var expectedMessage = "Last name cannot be empty.";
            var actualMessage = model.LastNameErrorMessage;

            Assert.NotEmpty(model.LastNameErrorMessage);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void CanSave_ProperArtist_ReturnsTrue()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.SetErrorMessageChangesAsync();
            model.InputModel.FirstName = "Scott";
            model.InputModel.LastName = "Snyder";
            
            Assert.True(model.CanSave);
        }

        [Fact]
        public void CanSave_WithoutFirstName_ReturnsFalse()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.InputModel.LastName = "Snyder";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanSave_WithoutLastName_ReturnsFalse()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.InputModel.FirstName = "Scott";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanSave_WithArtistError_ReturnsFalse()
        {
            var model = new AddEditArtistViewModel(null, null);
            model.InputModel.FirstName = "Scott";
            model.InputModel.LastName = "!";

            Assert.False(model.CanSave);

        }

        [Fact]
        public void CanSave_InputValuesEqualsEditedArtist_ReturnsFalse()
        {
            var model = new AddEditArtistViewModel(null, null);
            var artist = TestData.GetArtistSample().ToList()[1];
            model.SetErrorMessageChangesAsync();
            model.CheckPassedArtistAsync(artist);

            model.InputModel.FirstName = artist.FirstName;
            model.InputModel.LastName = artist.LastName;
            model.InputModel.Description = artist.Description;

            Assert.False(model.CanSave);
        }
    }
}

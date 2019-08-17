using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockQueryable.Moq;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class AddEditComicBookViewModelTests
    {
        [Fact]
        public void AddArtistCommand_ValidExecute()
        {
            var model = new AddEditComicBookViewModel(null, null, null, null );
            model.CheckPassedComicBookAsync(null);
            model.SelectedArtist = TestData.GetArtistSample().First();
            model.AddArtistCommand.Execute();


            Assert.NotEmpty(model.InputModel.ComicBookArtists);

        }

        [Fact]
        public void AddArtistCommand_AlreadyInCollection()
        {
            var model = new AddEditComicBookViewModel(null, null, null, null);
            model.CheckPassedComicBookAsync(null);
            model.SelectedArtist = TestData.GetArtistSample().First();
            model.AddArtistCommand.Execute();
            model.SelectedArtist = TestData.GetArtistSample().First();
            model.AddArtistCommand.Execute();

            Assert.NotEmpty(model.InputModel.ComicBookArtists);
            Assert.Single(model.InputModel.ComicBookArtists);
            Assert.Equal(model.SelectedArtist.Name, model.InputModel.ComicBookArtists[0].Artist.Name);
        }

        [Fact]
        public void RemoveArtist_ValidExecute()
        {
            var model = new AddEditComicBookViewModel(null, null, null, null);
            model.CheckPassedComicBookAsync(null);
            model.SelectedArtist = TestData.GetArtistSample().First();
            model.AddArtistCommand.Execute();
            model.SelectedComicBookArtist = model.InputModel.ComicBookArtists.First();

            model.RemoveArtistCommand.Execute();

            Assert.Empty(model.InputModel.ComicBookArtists);
        }

        [Fact]
        public void TitleErrorMessage_DisplaysProperly()
        {
            var model = new AddEditComicBookViewModel(null, null, null, null );
            model.CheckPassedComicBookAsync(null);
            model.SetErrorMessagesChangesAsync();
            model.InputModel.Title = " ";

            Assert.True(model.InputModel.HasErrors);
            Assert.False(model.CanSave);
            Assert.Equal(model.InputModel.GetFirstError("Title"), model.TitleErrorMessage);
        }

        [Fact]
        public void PriceErrorMessage_DisplaysProperly()
        {
            var model = new AddEditComicBookViewModel(null, null, null, null );
            model.CheckPassedComicBookAsync(null);
            model.SetErrorMessagesChangesAsync();
            model.InputModel.Price = -1;

            Assert.True(model.InputModel.HasErrors);
            Assert.False(model.CanSave);
            Assert.Equal("Please enter valid price.", model.PriceErrorMessage);
        }

        [Fact]
        public void QuantityErrorMessage_DisplaysProperly()
        {
            var model = new AddEditComicBookViewModel(null, null, null, null );
            model.CheckPassedComicBookAsync(null);
            model.SetErrorMessagesChangesAsync();

            model.InputModel.Quantity = -1;

            Assert.True(model.InputModel.HasErrors);
            Assert.False(model.CanSave);
            Assert.Equal("Please enter valid value.", model.QuantityErrorMessage);
        }

        [Fact]
        public void SaveComicBookCommand_Adding_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditComicBookViewModel>();
            model.CheckPassedComicBookAsync(null);
            model.SetErrorMessagesChangesAsync();
            var comic = TestData.GetComicBooksSample().First();

            model.InputModel.Title = comic.Title;
            model.InputModel.Series = comic.Series;
            model.InputModel.Price = comic.Price;
            model.InputModel.Quantity = comic.Quantity;
            model.InputModel.OnSaleDate = comic.OnSaleDate;
            model.SelectedArtist = comic.ComicBookArtists.First().Artist;
	    model.AddArtistCommand.Execute();

            model.SaveComicBookCommand.Execute();
            Assert.False(model.InputModel.HasErrors);
            Assert.True(model.CanSave);
            mock.Mock<IRepository<ComicBook>>().Verify(x => x.Add(model.ComicBook), Times.Once);
        }

        [Fact]
        public void SaveComicBookCommand_Editing_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditComicBookViewModel>();
            var comic = TestData.GetComicBooksSample().First();
            model.CheckPassedComicBookAsync(comic);
            model.SetErrorMessagesChangesAsync();
            model.InputModel.Title = "New title";
            model.InputModel.ComicBookArtists.First().Type = "Writer";
            Assert.Equal(true,model.ComicBook.ComicBookArtists.Any(x => x.Artist.FirstName == model.InputModel.ComicBookArtists.First().Artist.FirstName && x.Artist.LastName == model.InputModel.ComicBookArtists.First().Artist.LastName) );


            model.SaveComicBookCommand.Execute();

            mock.Mock<IRepository<ComicBook>>().Verify(x => x.Update(model.ComicBook), Times.Once);
            Assert.Equal(2, model.ComicBook.ComicBookArtists.Count);
        }

        [Fact]
        public async Task GetData_ValidCall()
        {
            var mockArtistData = TestData.GetArtistSample().BuildMock();
            var mockSeriesData = TestData.GetSeriesSample().BuildMock();
            var mock = AutoMock.GetLoose();
            mock.Mock<IRepository<Artist>>().Setup(x => x.GetAll()).Returns(mockArtistData.Object);
            mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(mockSeriesData.Object);
            var model = mock.Create<AddEditComicBookViewModel>();

            await model.GetDataAsync().ConfigureAwait(true);
            
            mock.Mock<IRepository<Series>>().Verify(x => x.GetAll(), Times.Once);
            mock.Mock<IRepository<Artist>>().Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public void GoBackCommand_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditComicBookViewModel>();
            model.GoBackCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "ComicBookList"), Times.Once);
        }

    }
}

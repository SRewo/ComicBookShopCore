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
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class AddEditComicBookViewModelTests
    {
        //[Fact]
        //public void AddArtistCommand_ValidExecute()
        //{
        //    var model = new AddEditComicBookViewModel(null, null, null, null, null);
        //    model.ComicBook = new ComicBook();
        //    model.ComicBook.ComicBookArtists = new ObservableCollection<ComicBookArtist>();
        //    model.SelectedArtist = new Artist(1)
        //    {
        //        FirstName = "Scott",
        //        LastName = "Snyder",
        //        Description = "Some random description"
        //    };
        //    model.AddArtistCommand.Execute();

        //    Assert.NotEmpty(model.ComicBook.ComicBookArtists);

        //}

        //[Fact]
        //public void AddArtistCommand_AlreadyInCollection()
        //{
        //    var model = new AddEditComicBookViewModel(null, null, null, null, null);
        //    model.ComicBook = new ComicBook();
        //    model.ComicBook.ComicBookArtists = new ObservableCollection<ComicBookArtist>();
        //    model.SelectedArtist = new Artist(1)
        //    {
        //        FirstName = "Scott",
        //        LastName = "Snyder",
        //        Description = "Some random description"
        //    };
        //    model.ComicBook.ComicBookArtists.Add(new ComicBookArtist() {
        //        Artist = model.SelectedArtist,
        //        ComicBook = model.ComicBook,
        //        Type = "Writer"
        //    });
        //    model.AddArtistCommand.Execute();

        //    Assert.NotEmpty(model.ComicBook.ComicBookArtists);
        //    Assert.Single(model.ComicBook.ComicBookArtists);
        //    Assert.Equal(model.SelectedArtist.Name, model.ComicBook.ComicBookArtists[0].Artist.Name);
        //}

        //[Fact]
        //public void RemoveArtist_ValidExecute()
        //{
        //    var model = new AddEditComicBookViewModel(null, null, null, null, null);
        //    model.ComicBook = new ComicBook();
        //    model.ComicBook.ComicBookArtists = new ObservableCollection<ComicBookArtist>();
        //    model.SelectedArtist = new Artist(1)
        //    {
        //        FirstName = "Scott",
        //        LastName = "Snyder",
        //        Description = "Some random description"
        //    };
        //    model.ComicBook.ComicBookArtists.Add(new ComicBookArtist()
        //    {
        //        Artist = model.SelectedArtist,
        //        ComicBook = model.ComicBook,
        //        Type = "Writer"
        //    });
        //    model.SelectedComicBookArtist = model.ComicBook.ComicBookArtists[0];
        //    model.RemoveArtistCommand.Execute();

        //    Assert.Empty(model.ComicBook.ComicBookArtists);
        //}

        //[Fact]
        //public void TitleErrorMessage_DisplaysProperly()
        //{
        //    var model = new AddEditComicBookViewModel(null, null, null, null, null);
        //    model.ComicBook = new ComicBook();
        //    model.ComicBook.ErrorsChanged += model.ComicBook_ErrorsChanged;
        //    model.ComicBook.Title = " ";

        //    Assert.True(model.ComicBook.HasErrors);
        //    Assert.False(model.CanSave);
        //    Assert.Equal("Comic book title cannot be empty.", model.TitleErrorMessage);
        //}

        //[Fact]
        //public void PriceErrorMessage_DisplaysProperly()
        //{
        //    var model = new AddEditComicBookViewModel(null, null, null, null, null);
        //    model.ComicBook = new ComicBook();
        //    model.ComicBook.ErrorsChanged += model.ComicBook_ErrorsChanged;
        //    model.ComicBook.Price = -1;

        //    Assert.True(model.ComicBook.HasErrors);
        //    Assert.False(model.CanSave);
        //    Assert.Equal("Please enter valid price.", model.PriceErrorMessage);
        //}

        //[Fact]
        //public void QuantityErrorMessage_DisplaysProperly()
        //{
        //    var model = new AddEditComicBookViewModel(null, null, null, null, null);
        //    model.ComicBook = new ComicBook();
        //    model.ComicBook.ErrorsChanged += model.ComicBook_ErrorsChanged;
        //    model.ComicBook.Quantity = -1;

        //    Assert.True(model.ComicBook.HasErrors);
        //    Assert.False(model.CanSave);
        //    Assert.Equal("Please enter valid value.", model.QuantityErrorMessage);
        //}

        //[Fact]
        //public void SaveComicBookCommand_Adding_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var model = mock.Create<AddEditComicBookViewModel>();
        //    model.ComicBook = new ComicBook();
        //    model.SaveComicBookCommand.Execute();

        //    mock.Mock<IRepository<ComicBook>>().Verify(x => x.Add(model.ComicBook), Times.Once);
        //}

        //[Fact]
        //public void SaveComicBookCommand_Editing_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var model = mock.Create<AddEditComicBookViewModel>();
        //    model.ComicBook = new ComicBook(1);
        //    model.GetComicBook(null);
        //    model.SaveComicBookCommand.Execute();

        //    mock.Mock<IRepository<ComicBook>>().Verify(x => x.Update(model.ComicBook), Times.Once);
        //}

        //[Fact]
        //public void GetData_ValidCall()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    mock.Mock<IRepository<Artist>>().Setup(x => x.GetAll()).Returns(GetSampleArtists);
        //    mock.Mock<IRepository<Series>>().Setup(x => x.GetAll()).Returns(GetSeriesSample);
        //    var model = mock.Create<AddEditComicBookViewModel>();
        //    model.GetData();

        //    mock.Mock<IRepository<Artist>>().Verify(x => x.GetAll(), Times.Once);
        //    mock.Mock<IRepository<Series>>().Verify(x => x.GetAll(), Times.Once);

        //    Assert.NotEmpty(model.Artists);
        //    Assert.NotEmpty(model.SeriesList);

        //    Assert.Equal(GetSampleArtists().Count(), model.Artists.Count);
        //    Assert.Equal(GetSeriesSample().Count(), model.SeriesList.Count);
        //}

        //[Fact]
        //public void GoBackCommand_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var model = mock.Create<AddEditComicBookViewModel>();
        //    model.ComicBook = new ComicBook();
        //    model.ComicBook.ComicBookArtists = new ObservableCollection<ComicBookArtist>();
        //    model.GoBackCommand.Execute();

        //    mock.Mock<IRepository<ComicBook>>().Verify(x => x.Reload(model.ComicBook), Times.Once);
        //    mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "ComicBookList"), Times.Once);
        //}

    }
}

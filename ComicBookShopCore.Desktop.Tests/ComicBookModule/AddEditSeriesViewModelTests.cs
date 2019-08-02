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
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class AddEditSeriesViewModelTests
    {
        //[Fact]
        //public void ResetForm_ValidCall()
        //{
        //    var model = new AddEditSeriesViewModel(null,null,null)
        //    {
        //        Series = new Series(),
        //        Publishers = new List<Publisher>(),
        //        NameErrorMessage = "Test"
        //    };
        //    model.ResetForm();

        //    Assert.Null(model.Series);
        //    Assert.Null(model.Publishers);
        //    Assert.Empty(model.NameErrorMessage);
        //}

        //[Fact]
        //public void GetPublishersFromRepository_ValidCall()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(GetPublishersSample);
        //    var model = mock.Create<AddEditSeriesViewModel>();
        //    model.GetPublishersFromRepository();

        //    mock.Mock<IRepository<Publisher>>().Verify(x => x.GetAll(), Times.Once);
        //    Assert.NotNull(model.Publishers);
        //    Assert.Equal(3, model.Publishers.Count);
        //    Assert.Equal("DC Comics", model.Publishers.First().Name);
        //}

        //[Fact]
        //public void SaveSeriesCommand_Adding_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var model = mock.Create<AddEditSeriesViewModel>();
        //    model.Series = new Series();
        //    model.SaveSeriesCommand.Execute();

        //    mock.Mock<IRepository<Series>>().Verify(x => x.Add(model.Series), Times.Once);
        //}

        //[Fact]
        //public void SaveSeriesCommand_Updating_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var model = mock.Create<AddEditSeriesViewModel>();
        //    model.Series = new Series(1);
        //    model.SaveSeriesCommand.Execute();

        //    mock.Mock<IRepository<Series>>().Verify(x => x.Update(model.Series), Times.Once);
        //}

        //[Fact]
        //public void GoBackCommand_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var model = mock.Create<AddEditSeriesViewModel>();
        //    model.GoBackCommand.Execute();

        //    mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "SeriesList"), Times.Once);
        //}

        //[Fact]
        //public void NameErrorMessage_SetsProperly()
        //{
        //    var model = new AddEditSeriesViewModel(null, null, null);
        //    model.Series = new Series();
        //    model.Series.ErrorsChanged += model.Series_ErrorsChanged;
        //    model.Series.Publisher = GetPublishersSample().First();
        //    model.Series.Name = " ";

        //    Assert.True(model.Series.HasErrors);
        //    Assert.Equal(model.Series.GetFirstError("Name"), model.NameErrorMessage);
        //}
        
        //[Fact]
        //public void CanSave_PublisherIsNull_ReturnsFalse()
        //{
        //    var model = new AddEditSeriesViewModel(null, null, null);
        //    model.Series = new Series();
        //    model.Series.PropertyChanged += model.CanSaveChanged;
        //    model.Series.Name = "Random Name";
        //    model.Series.Description = "Rand desc";

        //    Assert.False(model.CanSave);

        //}

        //[Fact]
        //public void CanSave_NameIsNull_ReturnsFalse()
        //{
        //    var model = new AddEditSeriesViewModel(null, null, null);
        //    model.Series = new Series();
        //    model.Series.PropertyChanged += model.CanSaveChanged;
        //    model.Series.Name = " ";
        //    model.Series.Publisher = GetPublishersSample().First();
        //    model.Series.Description = "Rand desc";

        //    Assert.False(model.CanSave);
        //}

    }
}

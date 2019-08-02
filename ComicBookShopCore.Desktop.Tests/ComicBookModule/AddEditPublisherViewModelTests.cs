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
    public class AddEditPublisherViewModelTests
    {
        //[Fact]
        //public void OnNavigatedTo_WithoutParameters_ValidCall()
        //{
        //    var model = new AddEditPublisherViewModel(null, null);
        //    model.OnNavigatedTo(new NavigationContext(null, null));

        //    Assert.NotNull(model.Publisher);
        //}

        //[Fact]
        //public void OnNavigatedTo_WithParameters_ValidCall()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var uri = new Uri("test?name=value", UriKind.Relative);
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //    };
        //    var parameters = new NavigationParameters()
        //    {
        //        {"publisher", publisher}
        //    };
        //    var model = new AddEditPublisherViewModel(null, null);


        //    IRegion region = new Region();
        //    mock.Mock<IRegionNavigationService>().Setup(x => x.Region).Returns(region);
        //    var navigationService = mock.Create<IRegionNavigationService>();


        //    var navigationContext = new NavigationContext(navigationService, uri, parameters);
        //    model.OnNavigatedTo(navigationContext);

        //    Assert.NotNull(model.Publisher);
        //    Assert.Equal(publisher.Name, model.Publisher.Name);
        //    Assert.Equal(publisher.Id, model.Publisher.Id);
        //}

        //[Fact]
        //public void NameErrorsMessage_DisplaysProperMessage()
        //{
        //    var model = new AddEditPublisherViewModel(null, null);
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //    };
        //    model.Publisher = publisher;
        //    model.Publisher.Name = "";
        //    model.Publisher_ErrorsChanged(null, null);

        //    var expectedMessage = publisher.GetFirstError();
        //    var actualMessage = model.NameErrorMessage;

        //    Assert.True(model.Publisher.HasErrors);
        //    Assert.Equal(expectedMessage, actualMessage);
        //}

        //[Fact]
        //public void DateErrorMessage_DisplaysProperMessage()
        //{
        //    var model = new AddEditPublisherViewModel(null, null);
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //    };
        //    model.Publisher = publisher;
        //    model.Publisher.ErrorsChanged += model.Publisher_ErrorsChanged;
        //    model.Publisher.CreationDateTime = DateTime.MaxValue;

        //    var expectedMessage = "You have to choose a date between 01.01.1900 and today";
        //    var actualMessage = model.DateErrorMessage;

        //    Assert.True(model.Publisher.HasErrors);
        //    Assert.Equal(expectedMessage, actualMessage);
        //}

        //[Fact]
        //public void GoBack_ValidCall()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var model = mock.Create<AddEditPublisherViewModel>();
        //    model.GoBackCommand.Execute();

        //    mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "PublisherList"), Times.Once);
        //}

        //[Fact]
        //public void SavePublisherCommand_Adding_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var publisher = new Publisher()
        //    {
        //        Name = "Dc Comics",
        //        CreationDateTime = new DateTime(2010, 01, 01)
        //    };
        //    var model = mock.Create<AddEditPublisherViewModel>();
        //    model.Publisher = publisher;
        //    model.SavePublisherCommand.Execute();

        //    mock.Mock<IRepository<Publisher>>().Verify(x => x.Add(publisher), Times.Once);
        //}

        //[Fact]
        //public void SavePublisherComand_Editing_ValidExecute()
        //{
        //    using var mock = AutoMock.GetLoose();
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //        CreationDateTime = new DateTime(2010, 01, 01)
        //    };
        //    var model = mock.Create<AddEditPublisherViewModel>();
        //    model.Publisher = publisher;
        //    model.SavePublisherCommand.Execute();

        //    mock.Mock<IRepository<Publisher>>().Verify(x => x.Update(publisher), Times.Once);
        //}

        //[Fact]
        //public void CanExecuteChanged_HasErrors_CanSaveIsFalse()
        //{
        //    var model = new AddEditPublisherViewModel(null, null);
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //        CreationDateTime = new DateTime(2010, 01, 01)
        //    };
        //    model.Publisher = publisher;
        //    model.Publisher.Name = "t";
        //    model.CanExecuteChanged(null, null);

        //    Assert.False(model.CanSave);
        //}

        //[Fact]
        //public void CanExecuteChanged_WithoutErrors_CanSaveIsTrue()
        //{
        //    var model = new AddEditPublisherViewModel(null, null);
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //    };
        //    model.Publisher = publisher;
        //    model.Publisher.CreationDateTime = new DateTime(2010, 01, 01);
        //    model.CanExecuteChanged(null, null);

        //    Assert.True(model.CanSave);
        //}

        //[Fact]
        //public void CanExecuteChanged_NameIsEmpty_CanSaveIsFalse()
        //{
        //    var model = new AddEditPublisherViewModel(null, null);
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //        CreationDateTime = new DateTime(2010, 01, 01)
        //    };
        //    model.Publisher = publisher;
        //    model.Publisher.Name = "";
        //    model.CanExecuteChanged(null, null);

        //    Assert.False(model.CanSave);
        //}

        //[Fact]
        //public void CanExecuteChanged_CreationDateTimeIsMin_CanSaveIsFalse()
        //{
        //    var model = new AddEditPublisherViewModel(null, null);
        //    var publisher = new Publisher(1)
        //    {
        //        Name = "Dc Comics",
        //        CreationDateTime = new DateTime(2010, 01, 01)
        //    };
        //    model.Publisher = publisher;
        //    model.Publisher.CreationDateTime = DateTime.MinValue;
        //    model.CanExecuteChanged(null, null);

        //    Assert.False(model.CanSave);
        //}
    }
}

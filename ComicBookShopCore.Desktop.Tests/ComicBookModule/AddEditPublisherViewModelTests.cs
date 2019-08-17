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
    public class AddEditPublisherViewModelTests
    {
        [Fact]
        public void OnNavigatedTo_ValidCall()
        {
            var mock = new Mock<AddEditPublisherViewModel>(null,null);
            var model = mock.Object;
            model.OnNavigatedTo(new NavigationContext(null, null));

            mock.Verify(x => x.CheckPassedPublisherAsync(null), Times.Once);
            mock.Verify(x => x.ResetModelAsync(), Times.Once);
            mock.Verify(x => x.SetErrorsChangedEventAsync(), Times.Once);
        }

        [Fact]
        public void CheckPassedPublisher_WithoutPublisher_ValidCall()
        {
            var model = new AddEditPublisherViewModel(null, null);
            model.CheckPassedPublisherAsync(null);

            Assert.Null(model.Publisher);
            Assert.Null(model.InputModel.Name);
            Assert.Null(model.InputModel.Description);
            Assert.NotEqual(DateTime.MinValue, model.InputModel.CreationDateTime);
        }

        [Fact]
        public void NameErrorsMessage_DisplaysProperMessage()
        {
            var model = new AddEditPublisherViewModel(null, null);
            model.CheckPassedPublisherAsync(null);
            model.SetErrorsChangedEventAsync();

            model.InputModel.Name = "";

            var expectedMessage = "Publisher name cannot be empty.";
            var actualMessage = model.NameErrorMessage;

            Assert.True(model.InputModel.HasErrors);
            Assert.False(model.CanSave);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void DateErrorMessage_DisplaysProperMessage()
        {

            var model = new AddEditPublisherViewModel(null, null);

            model.CheckPassedPublisherAsync(null);
            model.SetErrorsChangedEventAsync();
            model.InputModel.CreationDateTime = DateTime.MaxValue;

            var expectedMessage = "You have to choose a date between 01.01.1900 and today";
            var actualMessage = model.DateErrorMessage;

            Assert.True(model.InputModel.HasErrors);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public void GoBack_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditPublisherViewModel>();
            model.GoBackCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "PublisherList"), Times.Once);
        }

        [Fact]
        public void SavePublisherCommand_Adding_ValidExecute()
        {

            using var mock = AutoMock.GetLoose();
            var publisher = TestData.GetPublishersSample().First();
            var model = mock.Create<AddEditPublisherViewModel>();
            model.SetErrorsChangedEventAsync();
            model.CheckPassedPublisherAsync(null);

            model.InputModel.Name = publisher.Name;
            model.InputModel.Description = publisher.Description;
            model.InputModel.CreationDateTime = publisher.CreationDateTime;

            Assert.False(model.IsEditing);
            Assert.True(model.CanSave);
            model.SavePublisherCommand.Execute();

            mock.Mock<IRepository<Publisher>>().Verify(x => x.Add(model.Publisher), Times.Once);
        }

        [Fact]
        public void SavePublisherCommand_Editing_ValidExecute()
        {

            var publisher = TestData.GetPublishersSample().First();
            using var mock = AutoMock.GetLoose();
            var model = mock.Create<AddEditPublisherViewModel>();
            model.CheckPassedPublisherAsync(publisher);
            model.SetErrorsChangedEventAsync();

            model.InputModel.Name = "Test name";
            model.SavePublisherCommand.Execute();

            mock.Mock<IRepository<Publisher>>().Verify(x => x.Update(model.Publisher), Times.Once);
        }

        [Fact]
        public void CanExecuteChanged_HasErrors_CanSaveIsFalse()
        {
            var model = new AddEditPublisherViewModel(null, null);
            var publisher = TestData.GetPublishersSample().First();
            model.SetErrorsChangedEventAsync();
            model.InputModel.Name = "t";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanExecuteChanged_WithoutErrors_CanSaveIsTrue()
        {
            var model = new AddEditPublisherViewModel(null, null);
            var publisher = TestData.GetPublishersSample().First();
            model.CheckPassedPublisherAsync(publisher);
            model.SetErrorsChangedEventAsync();

            model.InputModel.CreationDateTime = new DateTime(2010, 01, 01);

            Assert.True(model.CanSave);
        }

        [Fact]
        public void CanExecuteChanged_NameIsEmpty_CanSaveIsFalse()
        {
            var model = new AddEditPublisherViewModel(null, null);
            var publisher = TestData.GetPublishersSample().First();
            model.CheckPassedPublisherAsync(publisher);
            model.SetErrorsChangedEventAsync();

            model.InputModel.Name = "";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanExecuteChanged_CreationDateTimeIsMin_CanSaveIsFalse()
        {
            var model = new AddEditPublisherViewModel(null, null);
            var publisher = TestData.GetPublishersSample().First();
            model.CheckPassedPublisherAsync(publisher);
            model.SetErrorsChangedEventAsync();

            model.Publisher.CreationDateTime = DateTime.MinValue;

            Assert.False(model.CanSave);
        }
        
    }
}

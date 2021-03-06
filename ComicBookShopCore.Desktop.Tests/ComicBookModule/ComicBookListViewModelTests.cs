﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class ComicBookListViewModelTests
    {

        [Fact]
        public void IsSearchEnabled_ViewListIsNull_EqualsFalse()
        {

            var model = new ComicBookListViewModel(null, null, null)
            {
                ViewList = null
            };

            Assert.False(model.IsSearchEnabled);

        }

        [Fact]
        public void IsSearchEnabled_ViewListNotNull_EqualsTrue()
        {

            var model = new ComicBookListViewModel(null, null, null)
            {
                ViewList = new List<ComicBook>()
            };
            model.CanSearchCheck();

            Assert.True(model.IsSearchEnabled);

        }

        [Fact]
        public void IsEditEnabled_SelectedComicBookIsNull_EqualsFalse()
        {
            var model = new ComicBookListViewModel(null, null, null);

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedComicBookNotNull_EqualsTrue()
        {
            var model = new ComicBookListViewModel(null, null, null)
            {
                SelectedComicBook = TestData.GetComicBooksSample().First()
            };

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedComicBookChanged_EqualsTrue()
        {

            var model = new ComicBookListViewModel(null, null, null);

            Assert.False(model.IsEditEnabled);

            model.SelectedComicBook = TestData.GetComicBooksSample().First(); 

            Assert.True(model.IsEditEnabled);

        }

        [Fact]
        public void OpenAddCommand_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditComicBook"));
            var model = mock.Create<ComicBookListViewModel>();
            model.AddComicBookCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditComicBook"), Times.Once);
        }

        [Fact]
        public void OpenEditCommand_ValidExecute()
        {
            using var mock = AutoMock.GetLoose();
            var comicBook = TestData.GetComicBooksSample().First(); 
            var navigationParameters = new NavigationParameters()
                {
                    {"ComicBook", comicBook }
                };
            mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditComicBook", navigationParameters));
            var model = mock.Create<ComicBookListViewModel>();
            model.SelectedComicBook = comicBook;
            model.EditComicBookCommand.Execute();

            mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditComicBook", navigationParameters), Times.Once);
        }

        [Fact]
        public void GetComicBooks_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var comicBooks = TestData.GetComicBooksSample();
            mock.Mock<IRepository<ComicBook>>().Setup(x => x.GetAll()).Returns(comicBooks);
            var model = mock.Create<ComicBookListViewModel>();
            model.GetComicBooks();

            var expectedCount = comicBooks.Count();
            var expectedFirstTitle = "Dark Nights Metal: #1";
            var actualCount = model.ViewList.Count;
            var actualFirstTitle = model.ViewList.First().Title;

            mock.Mock<IRepository<ComicBook>>().Verify(x => x.GetAll(), Times.AtLeast(1));
            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedFirstTitle, actualFirstTitle);
        }

        [Fact]
        public void GetPublishers_ValidCall()
        {
            using var mock = AutoMock.GetLoose();
            var publishers = TestData.GetPublishersSample();
            mock.Mock<IRepository<Publisher>>().Setup(x => x.GetAll()).Returns(publishers);
            var model = mock.Create<ComicBookListViewModel>();
            model.GetPublishers();

            var expectedCount = publishers.Count();
            var expectedFirstName = "DC Comics";
            var actualCount = model.Publishers.Count();
            var actualFirstName = model.Publishers.FirstOrDefault().Name;

            mock.Mock<IRepository<Publisher>>().Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(expectedCount, actualCount);
            Assert.Equal(expectedFirstName, actualFirstName);
        }

        [Fact]
        public void SearchWordChanged_ValidExecute()
        {
            var model = new ComicBookListViewModel(null, null, null)
            {
                AllComicBooks = TestData.GetComicBooksSample().ToList(),
                SearchWord = "Ant"
            };
            model.SearchWordChanged.Execute();

            Assert.NotEmpty(model.ViewList);
            Assert.Single(model.ViewList);
            Assert.Contains(model.AllComicBooks[1], model.ViewList);
        }

        [Fact]
        public void SelectedPublisherChanged_ValidExecute()
        {
            var model = new ComicBookListViewModel(null, null, null)
            {
                AllComicBooks = TestData.GetComicBooksSample().ToList(),
                SelectedPublisher = TestData.GetPublishersSample().First()
            };
            model.SearchWord = string.Empty;
            model.SelectedPublisherChanged.Execute();
            

            Assert.NotEmpty(model.ViewList);
            Assert.Single(model.ViewList);
            Assert.Contains(model.AllComicBooks[0], model.ViewList);
        }

        [Fact]
        public void PublisherAndSearchWordChanged_ValidExecute()
        {
            var model = new ComicBookListViewModel(null, null, null)
            {
                AllComicBooks = TestData.GetComicBooksSample().ToList(),
                SelectedPublisher = TestData.GetPublishersSample().First()
            };
            model.SearchWord = string.Empty;
            model.SelectedPublisherChanged.Execute();
            model.SearchWord = "Ant";
            model.SearchWordChanged.Execute();

            Assert.Empty(model.ViewList);
        }

        [Fact]
        public void SearchWordAndPublisherChanged_ValidExecute()
        {
            var model = new ComicBookListViewModel(null, null, null)
            {
                AllComicBooks = TestData.GetComicBooksSample().ToList(),
                SearchWord = "Ant"
            };
            model.SearchWordChanged.Execute();
            model.SelectedPublisher = TestData.GetPublishersSample().ToList()[1];
            model.SelectedPublisherChanged.Execute();

            Assert.NotEmpty(model.ViewList);
            Assert.Single(model.ViewList);
            Assert.Contains(model.AllComicBooks[1], model.ViewList);
        }

        [Fact]
        public void ResetSearchCommand_ValidExecute()
        {
            var model = new ComicBookListViewModel(null, null, null)
            {
                AllComicBooks = TestData.GetComicBooksSample().ToList(),
                SearchWord = "Ant"
            };
            model.SearchWordChanged.Execute();
            model.SelectedPublisher = TestData.GetPublishersSample().ToList()[1];
            model.SelectedPublisherChanged.Execute();
            model.ResetSearchCommand.Execute();

            Assert.Null(model.SelectedPublisher);
            Assert.Empty(model.SearchWord);
            Assert.Equal(model.AllComicBooks.Count, model.ViewList.Count);
        }
    }
}

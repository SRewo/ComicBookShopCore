using System;
using System.Collections.Generic;
using System.Text;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using Prism.Regions;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class PublisherListViewModelTests
    {
        [Fact]
        public void IsSearchEnabled_ViewListIsNull_EqualsFalse()
        {

            var model = new PublishersListViewModel(new RegionManager())
            {
                ViewList = null
            };

            Assert.False(model.IsSearchEnabled);

        }

        [Fact]
        public void IsSearchEnabled_ViewListNotNull_EqualsTrue()
        {

            var model = new PublishersListViewModel(new RegionManager())
            {
                ViewList = new List<Publisher>()
            };
            model.CanSearchCheck();

            Assert.True(model.IsSearchEnabled);

        }

        [Fact]
        public void IsEditEnabled_SelectedPublisherIsNull_EqualsFalse()
        {
            var model = new PublishersListViewModel(new RegionManager());

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedPublisherNotNull_EqualsTrue()
        {
            var model = new PublishersListViewModel(new RegionManager())
            {
                SelectedPublisher = new Publisher()
            };

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedPublisherChanged_EqualsTrue()
        {

            var model = new PublishersListViewModel(new RegionManager());

            Assert.False(model.IsEditEnabled);

            model.SelectedPublisher= new Publisher();

            Assert.True(model.IsEditEnabled);

        }

    }
}

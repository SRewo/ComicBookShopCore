using System;
using System.Collections.Generic;
using System.Text;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using Prism.Regions;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests
{
    public class ComicBookListViewModelTests
    {

        [Fact]
        public void IsSearchEnabled_ViewListIsNull_EqualsFalse()
        {

            var model = new ComicBookListViewModel(new RegionManager())
            {
                ViewList = null
            };

            Assert.False(model.IsSearchEnabled);

        }

        [Fact]
        public void IsSearchEnabled_ViewListNotNull_EqualsTrue()
        {

            var model = new ComicBookListViewModel(new RegionManager())
            {
                ViewList = new List<ComicBook>()
            };
            model.CanSearchCheck();

            Assert.True(model.IsSearchEnabled);

        }

        [Fact]
        public void IsEditEnabled_SelectedComicBookIsNull_EqualsFalse()
        {
            var model = new ComicBookListViewModel(new RegionManager());

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedComicBookNotNull_EqualsTrue()
        {
            var model = new ComicBookListViewModel(new RegionManager())
            {
                SelectedComicBook = new ComicBook()
            };

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedComicBookChanged_EqualsTrue()
        {

            var model = new ComicBookListViewModel(new RegionManager());
            model.SelectedComicBook = new ComicBook();

            Assert.True(model.IsEditEnabled);

        }

    }
}

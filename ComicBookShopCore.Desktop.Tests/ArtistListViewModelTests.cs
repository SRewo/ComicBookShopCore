using System;
using System.Collections.Generic;
using Xunit;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using Prism.Regions;

namespace ComicBookShopCore.Desktop.Tests
{
    public class ArtistListViewModelTests
    {
        [Fact]
        public void IsSearchEnabled_ViewListIsNull_EqualsFalse()
        {

            var model = new ArtistListViewModel(new RegionManager())
            {
                ViewList = null
            };

            Assert.False(model.IsSearchEnabled);

        }

        [Fact]
        public void IsSearchEnabled_ViewListNotNull_EqualsTrue()
        {

            var model = new ArtistListViewModel(new RegionManager())
            {
                ViewList = new List<Artist>()
            };
            model.CanSearchCheck();

            Assert.True(model.IsSearchEnabled);

        }

        [Fact]
        public void IsEditEnabled_SelectedArtistIsNull_EqualsFalse()
        {
            var model = new ArtistListViewModel(new RegionManager());

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedArtistNotNull_EqualsTrue()
        {
            var model = new ArtistListViewModel(new RegionManager())
            {
                SelectedArtist = new Artist()
            };

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedArtistChanged_EqualsTrue()
        {

            var model = new ArtistListViewModel(new RegionManager());
            model.SelectedArtist = new Artist();
            
            Assert.True(model.IsEditEnabled);

        }

    }
}

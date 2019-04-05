using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac.Extras.Moq;
using Xunit;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.ComicBookModule.Views;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Prism.Regions;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
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

            Assert.False(model.IsEditEnabled);

            model.SelectedArtist = new Artist();
            
            Assert.True(model.IsEditEnabled);

        }

        [Fact]
        public void GetData_ValidCall()
        {
            
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IRepository<Artist>>().Setup(x => x.GetAll()).Returns(GetSampleArtists());

                var model = mock.Create<ArtistListViewModel>();

                var expected = GetSampleArtists().ToList();

                model.GetTable();
                var actual = model.ViewList;

                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
            }

            
        }

        private IQueryable<Artist> GetSampleArtists()
        {
            List<Artist> testsArtists = new List<Artist>();
            {
                new Artist
                {
                    FirstName = "John",
                    LastName =  "Mick"
                };
                new Artist
                {
                    FirstName = "Mark",
                    LastName = "Well"
                };
            };

            return testsArtists.AsEnumerable().AsQueryable();
        }

    }
}

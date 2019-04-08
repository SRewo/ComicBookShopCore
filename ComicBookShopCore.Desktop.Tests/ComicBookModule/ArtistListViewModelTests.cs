using Autofac.Extras.Moq;
using ComicBookShopCore.ComicBookModule.ViewModels;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using Moq;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ComicBookShopCore.Desktop.Tests.ComicBookModule
{
    public class ArtistListViewModelTests
    {
        [Fact]
        public void IsSearchEnabled_ViewListIsNull_EqualsFalse()
        {

            var model = new ArtistListViewModel(null, null)
            {
                ViewList = null
            };

            Assert.False(model.IsSearchEnabled);

        }

        [Fact]
        public void IsSearchEnabled_ViewListNotNull_EqualsTrue()
        {

            var model = new ArtistListViewModel(null, null)
            {
                ViewList = new List<Artist>()
            };
            model.CanSearchCheck();

            Assert.True(model.IsSearchEnabled);

        }

        [Fact]
        public void IsEditEnabled_SelectedArtistIsNull_EqualsFalse()
        {
            var model = new ArtistListViewModel(null, null);

            Assert.False(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedArtistNotNull_EqualsTrue()
        {
            var model = new ArtistListViewModel(null, null)
            {
                SelectedArtist = new Artist()
            };

            Assert.True(model.IsEditEnabled);
        }

        [Fact]
        public void IsEditEnabled_SelectedArtistChanged_EqualsTrue()
        {

            var model = new ArtistListViewModel(null, null);

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

                Assert.NotNull(actual);
                Assert.Equal(expected.Count, actual.Count);
            }

        }

        [Fact]
        public void AddArtist_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                
                mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content","AddEditArtist"));

                var model = mock.Create<ArtistListViewModel>();
                model.OpenAdd();
               
               mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditArtist"), Times.Exactly(1));
            }
        }

        [Fact]
        public void EditArtist_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var parameters = new NavigationParameters();
                var artist = new Artist();
                parameters.Add("Artist",artist);

                mock.Mock<IRegionManager>().Setup(x => x.RequestNavigate("content", "AddEditArtist", parameters));

                var model = mock.Create<ArtistListViewModel>();
                model.SelectedArtist = artist;
                model.OpenEdit();

                mock.Mock<IRegionManager>().Verify(x => x.RequestNavigate("content", "AddEditArtist", parameters), Times.Exactly(1));

            }
        }

        [Fact]
        public void SearchWordChanged_ValidCall()
        {
            using (var mock = AutoMock.GetLoose())
            {

                mock.Mock<IRepository<Artist>>().Setup(x => x.GetAll()).Returns(GetSampleArtists());

                var model = mock.Create<ArtistListViewModel>();

                model.GetTable();

                model.SearchWord = "John";
                model.Search();

                var expected = 2;
                var actual = model.ViewList.Count;
                
                Assert.Equal(expected,actual);

                model.SearchWord = String.Empty;
                model.Search();

                expected = 3;
                actual = model.ViewList.Count;

                Assert.Equal(expected, actual);
                
            }
        }

        private IQueryable<Artist> GetSampleArtists()
        {

            var testsArtists = new List<Artist>
            {
                new Artist
                {
                    FirstName = "John",
                    LastName =  "Mick"
                },
                new Artist
                {
                    FirstName = "Mark",
                    LastName = "Well"
                },
                new Artist
                {
                    FirstName = "John",
                    LastName = "River"
                }
            };

            return testsArtists.AsEnumerable().AsQueryable();

        }

    }
}

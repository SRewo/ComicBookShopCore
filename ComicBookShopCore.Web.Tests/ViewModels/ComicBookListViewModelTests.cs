using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Web.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xunit;

namespace ComicBookShopCore.Web.Tests.ViewModels
{
    public class ComicBookListViewModelTests
    {
        [Fact]
        public void GetData_GetAll_ValidCall()
        {
            var mock = new Mock<IRepository<ComicBook>>();
            mock.Setup(x => x.GetAll()).Returns(GetComicBooksSample());
            var model = new ComicBookListViewModel(mock.Object, 0, 1);
            model.GetData();

            Assert.NotEmpty(model.ViewList);
            Assert.Equal(1, model.Page);
            Assert.Equal(1, model.NumberOfPages);
        }

        [Fact]
        public void GetData_PublisherIsSelected_ValidCall()
        {
            var mock = new Mock<IRepository<ComicBook>>();
            mock.Setup(x => x.GetAll()).Returns(GetComicBooksSample());
            var model = new ComicBookListViewModel(mock.Object, 1, 1);
            model.GetData();

            Assert.NotEmpty(model.ViewList);
            Assert.Equal(1, model.NumberOfPages);
            Assert.Single(model.ViewList);
        }

        [Fact]
        public void GetData_OtherPublishersSelected_ValidCall()
        {
            var mock = new Mock<IRepository<ComicBook>>();
            mock.Setup(x => x.GetAll()).Returns(GetComicBooksSample);
            var model = new ComicBookListViewModel(mock.Object, 4, 1);

            Assert.Null(model.ViewList);
        }


        private IQueryable<ComicBook> GetComicBooksSample()
        {
            var series = GetSeriesSample().ToList();
            var artists = GetSampleArtists().ToList();

            var comicBooks = new List<ComicBook>()
            {
                new ComicBook()
                {
                    Title = "Dark Nights Metal: #1",
                    Series = series[0],
                    Price = 9.99,
                    Quantity = 10,
                    OnSaleDate = new DateTime(2010,12,10),
                    ComicBookArtists = new ObservableCollection<ComicBookArtist>()
                    {
                        new ComicBookArtist()
                        {
                            Artist = artists[0],
                            Type = "Writer"
                        },
                        new ComicBookArtist()
                        {
                            Artist = artists[1],
                            Type = "Cover Variant"
                        },
                    }
                },
                new ComicBook()
                {
                    Title = "Ant Man Last Days: #1",
                    Series = series[1],
                    Price = 5.99,
                    Quantity = 15,
                    OnSaleDate = new DateTime(2011,10,20),
                    ComicBookArtists = new ObservableCollection<ComicBookArtist>()
                    {
                        new ComicBookArtist()
                        {
                            Artist = artists[2],
                            Type = "Writer"
                        },
                        new ComicBookArtist()
                        {
                            Artist = artists[1],
                            Type = "Cover Variant"
                        },
                    }
                }
            };
            return comicBooks.AsQueryable();
        }

        private IQueryable<Series> GetSeriesSample()
        {
            var publishers = GetPublishersSample().ToList();
            var series = new List<Series>()
            {
                new Series(1)
                {
                    Name = "Dark Nights Metal",
                    Description = "Series one",
                    Publisher = publishers[0]
                },
                new Series(2)
                {
                    Name = "Ant-Man: Last Days",
                    Description = "Ant-Man",
                    Publisher = publishers[1]
                }
            };

            return series.AsQueryable();

        }

        private IQueryable<Publisher> GetPublishersSample()
        {
            var tmp = new List<Publisher>()
            {
                new Publisher(1)
                {
                    Name = "DC Comics",
                    CreationDateTime = DateTime.Parse("01.01.1934"),
                    Description = "Some random description."
                },
                new Publisher(2)
                {
                    Name = "Marvel Comics",
                    CreationDateTime = DateTime.Parse("01.01.1939"),
                    Description = "Another description"
                },
                new Publisher(3)
                {
                    Name = "Dark Horse Comics",
                    CreationDateTime = DateTime.Parse("01.01.1986"),
                    Description = " American comic book and manga publisher."
                }
            };

            return tmp.AsEnumerable().AsQueryable();
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

            return testsArtists.AsQueryable();

        }
    }
}

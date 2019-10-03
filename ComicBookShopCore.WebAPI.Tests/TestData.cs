using System;
using System.Collections.Generic;
using System.Linq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Builders;

namespace ComicBookShopCore.WebAPI.Tests
{
    public static class TestData
    {
        public static IQueryable<Order> OrderSample()
        {
            var orderBuilder = new OrderBuilder();
            var orders = new List<Order>
            {
                orderBuilder
                    .Details
                    .User(UserSample().ToList()[1])
                    .Date(new DateTime(2019, 07, 07))
                    .Build()
            };
            orderBuilder = new OrderBuilder();
            orders.Add( orderBuilder
                    .Details
                    .User(UserSample().ToList()[0])
                    .Date(new DateTime(2019, 07, 20))
                    .Build());
            return orders.AsEnumerable().AsQueryable();
        }

        public static List<User> UserSample()
        {
            var users = new List<User>
            {
                new User
                {
                    FirstName = "John",
                    LastName = "Kent"
                },
                new User
                {
                    FirstName = "Martin",
                    LastName = "Won"
                }
            };
            return users;
        }

        public static IQueryable<ComicBook> GetComicBooksSample()
        {
            var series = GetSeriesSample().ToList();
            var artists = GetArtistSample().ToList();
            var comicBookBuilder = new ComicBookBuilder();

            var comicBooks = new List<ComicBook>
            {
                comicBookBuilder
                    .Details
                    .Title("Dark Nights Metal: #1")
                    .Series(series[0])
                    .OnSaleDate(new DateTime(2010, 12, 10))
                    .Status
                    .Quantity(10)
                    .Price(9.99)
                    .AddArtist
                    .Artist(artists[0])
                    .Role("Writer")
                    .Add()
                    .AddArtist
                    .Artist(artists[1])
                    .Role("Cover Variant")
                    .Add()
                    .Build()
            };
            comicBookBuilder = new ComicBookBuilder();

            comicBooks.Add(comicBookBuilder
                .Details
                .Title("Ant Man Last Days: #1")
                .Series(series[1])
                .OnSaleDate(new DateTime(2011, 10, 20))
                .Status
                .Quantity(15)
                .Price(5.99)
                .AddArtist
                .Artist(artists[2])
                .Role("Writer")
                .Add()
                .AddArtist
                .Artist(artists[1])
                .Role("Cover Variant")
                .Add()
                .Build());
            return comicBooks.AsQueryable();
        }

        public static IQueryable<Series> GetSeriesSample()
        {
            var publishers = GetPublishersSample().ToList();
            var seriesBuilder = new SeriesBuilder();
            var series = new List<Series>
            {
                seriesBuilder
                    .Details
                    .Name("Dark Nights Metal")
                    .Description("Series one")
                    .Publisher(publishers[0])
                    .Build()
            };
            seriesBuilder = new SeriesBuilder();

            series.Add(seriesBuilder
                .Details
                .Name("Ant-Man: Last Days")
                .Description("Ant-Man")
                .Publisher(publishers[1])
                .Build());
            return series.AsQueryable();
        }

        public static IQueryable<Publisher> GetPublishersSample()
        {
            var publisherBuilder = new PublisherBuilder();

            var testPublishers = new List<Publisher>
            {
                publisherBuilder
                    .Details
                    .Name("DC Comics")
                    .Created(DateTime.Parse("01.01.1934"))
                    .Description("Some random description.")
                    .Build()
            };

            publisherBuilder = new PublisherBuilder();

            testPublishers.Add(publisherBuilder
                .Details
                .Name("Marvel Comics")
                .Created(DateTime.Parse("01.01.1939"))
                .Description("Another description")
                .Build());

            publisherBuilder = new PublisherBuilder();

            testPublishers.Add(publisherBuilder
                .Details
                .Name("Dark Horse Comics")
                .Created(DateTime.Parse("01.01.1986"))
                .Description("American comic book and manga publisher.")
                .Build());

            return testPublishers.AsEnumerable().AsQueryable();
        }

        public static IQueryable<Artist> GetArtistSample()
        {
            var artistBuilder = new ArtistBuilder();

            var testsArtists = new List<Artist>
            {
                artistBuilder
                    .Details
                    .FirstName("John")
                    .LastName("Mick")
                    .Build()
            };

            artistBuilder = new ArtistBuilder();

            testsArtists.Add(artistBuilder
                .Details
                .FirstName("Mark")
                .LastName("Well")
                .Build());


            artistBuilder = new ArtistBuilder();

            testsArtists.Add(artistBuilder
                .Details
                .FirstName("John")
                .LastName("River")
                .Build());

            return testsArtists.AsQueryable();
        }
    }
}
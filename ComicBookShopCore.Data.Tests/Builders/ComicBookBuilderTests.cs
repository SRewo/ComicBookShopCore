using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ComicBookShopCore.Data.Builders;
using Xunit;

namespace ComicBookShopCore.Data.Tests.Builders
{
    public class ComicBookBuilderTests
    {
        [Fact]
        public void Build_ValidCall()
        {
            var builder = new ComicBookBuilder();
            var series = new Series();
            var artist = new ObservableCollection<ComicBookArtist>();

            var bc = builder
                .Details
                    .Series(series)
                    .Artists(artist)
                    .Title("Some Title")
                    .OnSaleDate(new DateTime(1999, 01, 01))
                .Description
                    .ShortDesc("Desc123")
                    .LongDesc("desc231213123213123")
                .Status
                    .Price(2.55)
                    .Quantity(20)
                .Build();

            Assert.NotNull(bc);
            Assert.Equal("Some Title", bc.Title);
        }

        [Fact]
        public void Build_MissingProperty_ThrowsException()
        {
            var builder = new ComicBookBuilder();
            var series = new Series();
            var artist = new ObservableCollection<ComicBookArtist>();

            var bc = builder
                .Details
                    .Series(series)
                    .Artists(artist)
                    .Title("Some Title")
                    .OnSaleDate(new DateTime(1999, 01, 01))
                .Description
                    .ShortDesc("Desc123")
                    .LongDesc("desc231213123213123")
                .Status
                    .Quantity(20);

            Assert.Throws<ValidationException>((() => bc.Build()));
        }

        [Fact]
        public void Build_InvalidPropertyValue_ThrowsException()
        {
            var builder = new ComicBookBuilder();
            var series = new Series();
            var artist = new ObservableCollection<ComicBookArtist>();

            var bc = builder
                .Details
                    .Series(series)
                    .Artists(artist)
                    .Title("Some Title")
                    .OnSaleDate(new DateTime(1999, 01, 01))
                .Description
                    .ShortDesc("Desc123")
                    .LongDesc("desc231213123213123")
                .Status
                    .Price(-2.22)
                    .Quantity(20);

            Assert.Throws<ValidationException>((() => bc.Build()));
        }
    }
}
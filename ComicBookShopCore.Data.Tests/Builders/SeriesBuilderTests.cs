using System;
using System.ComponentModel.DataAnnotations;
using ComicBookShopCore.Data.Builders;
using Xunit;

namespace ComicBookShopCore.Data.Tests.Builders
{
    public class SeriesBuilderTests
    {
        [Fact]
        public void Build_WithoutPublisher_ThrowsException()
        {
            var builder = new SeriesBuilder();
            var bc = builder.Details.Name("Series").Description("Desc");

            var ex = Assert.Throws<ValidationException>(() => bc.Build());
        }

        [Fact]
        public void Build_ValidCall()
        {
            var builder= new SeriesBuilder();
            var publisher = new PublisherBuilder()
                .Details
                    .Name("DC Comics")
                    .Created(new DateTime(1997,01,01))
                .Build();
            var bc = builder
                .Details
                    .Name("Series")
                    .Description("Desc")
                    .Publisher(publisher)
                .Build();

            Assert.NotNull(bc);
        }

        [Fact]
        public void Build_NameValidationError_ThrowsException()
        {
            var builder = new SeriesBuilder();
            var publisher = new PublisherBuilder()
                .Details
                    .Name("DC Comics")
                    .Created(new DateTime(1997, 01, 01))
                .Build();
            var bc = builder
                .Details
                    .Publisher(publisher)
                    .Description("Desc")
                    .Name("");

            var ex = Assert.Throws<ValidationException>(() => bc.Build());
            Assert.Equal("Series name cannot be empty", ex.Message);
        }
    }
}
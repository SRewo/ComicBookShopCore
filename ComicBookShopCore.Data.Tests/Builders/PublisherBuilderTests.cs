using System;
using System.ComponentModel.DataAnnotations;
using ComicBookShopCore.Data.Builders;
using Xunit;

namespace ComicBookShopCore.Data.Tests.Builders
{
    public class PublisherBuilderTests
    {
        [Fact]
        public void Build_NameValidationError_ThrowsException()
        {
            var builder = new PublisherBuilder();
            var bc = builder
                .Details
                    .Name("DC")
                    .Created(new DateTime(1999, 01, 01))
                    .Description("Test desc");

            var ex = Assert.Throws<ValidationException>(() => bc.Build());
            Assert.Equal("Publisher name is too short.", ex.Message);
        }

        [Fact]
        public void Build_MissingProperty_ThrowsException()
        {
            var builder = new PublisherBuilder();
            var bc = builder
                .Details
                    .Created(new DateTime(1999, 01, 10))
                    .Description("Desc");

            var ex = Assert.Throws<ValidationException>((() => bc.Build()));
            Assert.Equal("Publisher name cannot be empty", ex.Message);
        }

        [Fact]
        public void Build_ValidCall()
        {
            var builder = new PublisherBuilder();

            var publisher = builder
                .Details
                    .Name("DC Comics")
                    .Created(new DateTime(1999, 01, 01))
                    .Description("Desc")
                .Build();

            Assert.NotNull(publisher);
            Assert.False(publisher.HasErrors);
        }
    }
}
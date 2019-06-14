using System.ComponentModel.DataAnnotations;
using ComicBookShopCore.Data.Builders;
using Xunit;

namespace ComicBookShopCore.Data.Tests.Builders
{
    public class ArtistBuilderTests
    {
        [Fact]
        public void Build_InvalidName_ThrowsException()
        {
            var builder = new ArtistBuilder();
            var bc = builder
                .Details
                    .FirstName("-Jan")
                    .LastName("Testowy")
                    .Description("Random Desc");

            var ex = Assert.Throws<ValidationException>(() => bc.Build());

            Assert.Equal("First name cannot contain special characters.", ex.Message);
        }

        [Fact]
        public void Build_ValidCall()
        {
            var builder = new ArtistBuilder();
            var artist = builder
                .Details
                    .FirstName("Jan")
                    .LastName("Testowy")
                    .Description("desc")
                .Build();

            Assert.NotNull(artist);
            Assert.False(artist.HasErrors);
        }

        [Fact]
        public void Build_MissingProperty_ThrowsException()
        {
            var builder = new ArtistBuilder();
            var bc = builder
                .Details
                    .LastName("Testowy")
                    .Description("Desc");
            
            var ex = Assert.Throws<ValidationException>(() => bc.Build());
            Assert.Equal("First name cannot be empty.", ex.Message);
        }

        [Fact]
        public void Build_WithoutDescription_ValidCall()
        {
            var builder = new ArtistBuilder();
            var artist = builder
                .Details
                    .FirstName("Jan")
                    .LastName("Testowy")
                .Build();

            Assert.NotNull(artist);
            Assert.False(artist.HasErrors);
        }
    }

}
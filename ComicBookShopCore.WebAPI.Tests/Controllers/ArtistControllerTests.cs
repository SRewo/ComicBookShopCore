using Autofac.Extras.Moq;
using Xunit;
using MockQueryable.Moq;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data;
using ComicBookShopCore.WebAPI.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ComicBookShopCore.WebAPI.Tests.Controllers
{
    public class ArtistControllerTests
    {
	[Fact]
	public async Task Get_ReturnsArtists()
        {
	    var mock = AutoMock.GetLoose();
	    var testData = TestData.GetArtistSample();
	    mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetAllAsync()).Returns(Task.FromResult(testData.ToList().AsEnumerable()));
	    var controller = mock.Create<ArtistController>();
	    var actionResult = await controller.Get();
	    Assert.Equal(testData.Count(), actionResult.Count());
        }

        [Fact]
        public async Task GetArtist_ProperId_ReturnsArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
            mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult(artist));
            var controller = mock.Create<ArtistController>();
            var result = await controller.GetArtist(1);
            var resultArtist = Assert.IsType<Artist>(result.Value);
	    Assert.Equal(artist.FirstName, resultArtist.FirstName);
	    Assert.Equal(artist.LastName, resultArtist.LastName);
        }

        [Fact]
        public async Task GetArtist_InvalidId_ReturnsErrorCode()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult((Artist) null));
	    var controller = mock.Create<ArtistController>();
            var result = await controller.GetArtist(1);
	    Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_AddsArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
            var controller = mock.Create<ArtistController>();
	    var result = await controller.Post(artist);
	    Assert.IsType<CreatedResult>(result);
	    mock.Mock<IAsyncRepository<Artist>>().Verify(x => x.AddAsync(artist), Times.Once);
        }

        [Fact]
        public async Task Post_InvalidArtist_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
            artist.FirstName = "";
            var controller = mock.Create<ArtistController>();
            var result = await controller.Post(artist);
	    Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_DeletesArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
	    mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult(artist));
            var controller = mock.Create<ArtistController>();
            var result = await controller.Delete(1);
	    Assert.IsType<NoContentResult>(result);
	    mock.Mock<IAsyncRepository<Artist>>().Verify(x => x.DeleteAsync(artist), Times.Once);
        }

        [Fact]
        public async Task Delete_ArtistNotFound_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult((Artist) null));
	    var controller = mock.Create<ArtistController>();
            var result = await controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_UpdatesArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
            mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult(artist));
            var controller = mock.Create<ArtistController>();
            var result = await controller.Put(1, artist);
	    Assert.IsType<CreatedResult>(result);
            mock.Mock<IAsyncRepository<Artist>>().Verify(x => x.UpdateAsync(artist), Times.Once);
        }

        [Fact]
        public async Task Put_InvalidArtist_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
            mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult(artist));
	    artist.FirstName = "";
            var controller = mock.Create<ArtistController>();
            var result = await controller.Put(1, artist);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ArtistNotFound_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
            mock.Mock<IAsyncRepository<Artist>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult((Artist) null));
            var controller = mock.Create<ArtistController>();
            var result = await controller.Put(1, artist);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

using System;
using Autofac.Extras.Moq;
using Xunit;
using MockQueryable.Moq;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data;
using ComicBookShopCore.WebAPI.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ComicBookShopCore.Services.Artist;
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
	    var testData = new List<ArtistDto>()
            {
	        new ArtistDto(){FirstName = "Adam", LastName = "Test"}
            };
	    mock.Mock<IArtistService>().Setup(x => x.ListAsync()).Returns(Task.FromResult(testData.AsEnumerable()));
	    var controller = mock.Create<ArtistController>();
	    var actionResult = await controller.Get();
	    Assert.Equal(testData.Count(), actionResult.Count());
        }

        [Fact]
        public async Task GetArtist_ProperId_ReturnsArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = new ArtistDetailsDto(){FirstName = "Adam", LastName = "Test"};
            mock.Mock<IArtistService>().Setup(x => x.DetailsAsync(1)).Returns(Task.FromResult(artist));
            var controller = mock.Create<ArtistController>();
            var result = await controller.GetArtist(1);
            var resultArtist = Assert.IsType<ArtistDetailsDto>(result.Value);
	    Assert.Equal(artist.FirstName, resultArtist.FirstName);
	    Assert.Equal(artist.LastName, resultArtist.LastName);
        }

        [Fact]
        public async Task GetArtist_InvalidId_ReturnsErrorCode()
        {
            var mock = AutoMock.GetLoose();
	    var controller = mock.Create<ArtistController>();
            var result = await controller.GetArtist(1);
	    Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_AddsArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = new ArtistDetailsDto(){FirstName = "Adam", LastName = "Test"};
            var controller = mock.Create<ArtistController>();
	    var result = await controller.Post(artist);
	    Assert.IsType<CreatedResult>(result);
	    mock.Mock<IArtistService>().Verify(x => x.AddArtistAsync(artist));
        }

        [Fact]
        public async Task Post_InvalidArtist_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var artist = new ArtistDetailsDto(){FirstName = "", LastName = "Test"};
            var controller = mock.Create<ArtistController>();
            mock.Mock<IArtistService>().Setup(x => x.AddArtistAsync(artist)).Throws<ValidationException>();
            var result = await controller.Post(artist);
	    Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_DeletesArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = TestData.GetArtistSample().First();
            var controller = mock.Create<ArtistController>();
            var result = await controller.Delete(1);
	    Assert.IsType<NoContentResult>(result);
	    mock.Mock<IArtistService>().Verify(x => x.DeleteArtistAsync(1), Times.Once);
        }

        [Fact]
        public async Task Delete_ArtistNotFound_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IArtistService>().Setup(x => x.DeleteArtistAsync(1)).Throws(new NullReferenceException());
	    var controller = mock.Create<ArtistController>();
            var result = await controller.Delete(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_UpdatesArtist()
        {
            var mock = AutoMock.GetLoose();
            var artist = new ArtistDetailsDto(){FirstName = "Adam", LastName = "Test"};
            var controller = mock.Create<ArtistController>();
            var result = await controller.Put(1, artist);
	    Assert.IsType<CreatedResult>(result);
            mock.Mock<IArtistService>().Verify(x => x.UpdateArtistAsync(1,artist), Times.Once);
        }

        [Fact]
        public async Task Put_InvalidArtist_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var artist = new ArtistDetailsDto(){FirstName = "", LastName = "Test"};
            mock.Mock<IArtistService>().Setup(x => x.UpdateArtistAsync(1, artist)).Throws<ValidationException>();
            var controller = mock.Create<ArtistController>();
            var result = await controller.Put(1, artist);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ArtistNotFound_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var artist = new ArtistDetailsDto(){FirstName = "Adam", LastName = "Test"};
            mock.Mock<IArtistService>().Setup(x => x.UpdateArtistAsync(1,artist)).Throws<NullReferenceException>();
            var controller = mock.Create<ArtistController>();
            var result = await controller.Put(1, artist);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

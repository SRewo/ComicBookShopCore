using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ComicBookShopCore.Services.Series;
using ComicBookShopCore.WebAPI.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moq;
using Xunit;

namespace ComicBookShopCore.WebAPI.Tests.Controllers
{
    public class SeriesControllerTests
    {
        [Fact]
        public async Task Get_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Get();

            Assert.IsType<OkObjectResult>(result.Result);
            mock.Mock<ISeriesService>().Verify(x => x.SeriesListAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var series = new SeriesDetailsDto();
            mock.Mock<ISeriesService>().Setup(x => x.DetailsAsync(1)).Returns(Task.FromResult(series));
            var controller = mock.Create<SeriesController>();

            var result = await controller.GetById(1);

	    mock.Mock<ISeriesService>().Verify(x => x.DetailsAsync(1), Times.Once);
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<SeriesController>();

            var result = await controller.GetById(1);

	    mock.Mock<ISeriesService>().Verify(x => x.DetailsAsync(1), Times.Once);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ValidCall()
        {
            var mock = AutoMock.GetLoose();
	    var series = new SeriesInputDto();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Post(series);

	    mock.Mock<ISeriesService>().Verify(x => x.AddSeriesAsync(series), Times.Once);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Post_AddThrowsException_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
	    var series = new SeriesInputDto();
            mock.Mock<ISeriesService>().Setup(x => x.AddSeriesAsync(series)).Throws<ValidationException>();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Post(series);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ValidCall()
        {
            var mock = AutoMock.GetLoose();
	    var series = new SeriesInputDto();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Put(1, series);

            mock.Mock<ISeriesService>().Verify(x => x.UpdateSeriesAsync(1, series), Times.Once );
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Put_UpdateThrowsNullReferenceException_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
	    var series = new SeriesInputDto();
            mock.Mock<ISeriesService>().Setup(x => x.UpdateSeriesAsync(1, series)).Throws<NullReferenceException>();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Put(1, series);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_UpdateThrowsValidationException_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
	    var series = new SeriesInputDto();
            mock.Mock<ISeriesService>().Setup(x => x.UpdateSeriesAsync(1, series)).Throws<ValidationException>();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Put(1, series);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Delete(1);

	    mock.Mock<ISeriesService>().Verify(x => x.DeleteSeriesAsync(1), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_DeleteSeriesThrowsNullReferenceException_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<ISeriesService>().Setup(x => x.DeleteSeriesAsync(1)).Throws<NullReferenceException>();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Patch_ValidCall()
        {
            var mock = AutoMock.GetLoose();
	    var series = new SeriesInputDto();
            mock.Mock<ISeriesService>().Setup(x => x.EditSeriesAsync(1)).Returns(Task.FromResult(series));
            var controller = mock.Create<SeriesController>();

            var result = await controller.Patch(1, new JsonPatchDocument<SeriesInputDto>());

	    mock.Mock<ISeriesService>().Verify(x => x.UpdateSeriesAsync(1, series), Times.Once);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Patch_SeriesIsNull_ReturnsNotFoundException()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Patch(1, new JsonPatchDocument<SeriesInputDto>());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Patch_UpdateThrowsValidationException_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
            var series = new SeriesInputDto();
            mock.Mock<ISeriesService>().Setup(x => x.EditSeriesAsync(1)).Returns(Task.FromResult(series));
            mock.Mock<ISeriesService>().Setup(x => x.UpdateSeriesAsync(1, series)).Throws<ValidationException>();
            var controller = mock.Create<SeriesController>();

            var result = await controller.Patch(1, new JsonPatchDocument<SeriesInputDto>());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
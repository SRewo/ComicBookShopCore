using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ComicBookShopCore.Services.ComicBook;
using ComicBookShopCore.WebAPI.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ComicBookShopCore.WebAPI.Tests.Controllers
{
    public class ComicBookControllerTests
    {
        [Fact]
        public async Task Get_WithoutId_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var list = new List<ComicBookListDto>() {new ComicBookListDto()};
            mock.Mock<IComicBookService>().Setup(x => x.ComicListAsync()).Returns(Task.FromResult(list.AsEnumerable()));
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Get_WithoutList_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Get();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Get_WithId_ValidCall()
        {
            var mock = AutoMock.GetLoose();
	    var comic = new ComicBookDetailsDto();
            mock.Mock<IComicBookService>().Setup(x => x.ComicDetailsAsync(1)).Returns(Task.FromResult(comic));
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Get_InvalidId_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Get(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ValidCall()
        {
            var mock = AutoMock.GetLoose();
	    var comic = new ComicBookInputDto();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Post(comic);

	    mock.Mock<IComicBookService>().Verify(x => x.AddComicAsync(comic), Times.Once);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Post_AddComicThrowsValidationException_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
	    var comic = new ComicBookInputDto();
            mock.Mock<IComicBookService>().Setup(x => x.AddComicAsync(comic)).Throws(new ValidationException("error"));
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Post(comic);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Put_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<ComicBookController>();
            var comic = new ComicBookInputDto();

            var result = await controller.Put(1, comic);

	    mock.Mock<IComicBookService>().Verify(x => x.UpdateComicAsync(1,comic), Times.Once);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Put_UpdateThrowsNullReferenceException_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
	    var comic = new ComicBookInputDto();
            mock.Mock<IComicBookService>().Setup(x => x.UpdateComicAsync(1, comic)).Throws<NullReferenceException>();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Put(1, comic);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_UpdateThrowsValidationException_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
	    var comic = new ComicBookInputDto();
            mock.Mock<IComicBookService>().Setup(x => x.UpdateComicAsync(1, comic)).Throws<ValidationException>();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Put(1, comic);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Patch_ValidCall()
        {
            var mock = AutoMock.GetLoose();
	    var comic = new ComicBookInputDto();
            mock.Mock<IComicBookService>().Setup(x => x.ComicToEditAsync(1)).Returns(Task.FromResult(comic));
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Patch(1, new JsonPatchDocument<ComicBookInputDto>());

	    mock.Mock<IComicBookService>().Verify(x => x.UpdateComicAsync(1, comic), Times.Once);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Patch_ComicIsNull_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Patch(1, new JsonPatchDocument<ComicBookInputDto>());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Patch_UpdateThrowsValidationException_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
	    var comic = new ComicBookInputDto();
            mock.Mock<IComicBookService>().Setup(x => x.ComicToEditAsync(1)).Returns(Task.FromResult(comic));
            mock.Mock<IComicBookService>().Setup(x => x.UpdateComicAsync(1, comic)).Throws<ValidationException>();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Patch(1, new JsonPatchDocument<ComicBookInputDto>());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ValidCall()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Delete(1);

	    mock.Mock<IComicBookService>().Verify(x => x.DeleteComicAsync(1), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_DeleteComicThrowsNullReferenceException_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IComicBookService>().Setup(x => x.DeleteComicAsync(1)).Throws<NullReferenceException>();
            var controller = mock.Create<ComicBookController>();

            var result = await controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
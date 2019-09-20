using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Services.Publisher;
using ComicBookShopCore.WebAPI.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.NavigationExpansion.Internal;
using Moq;
using Xunit;

namespace ComicBookShopCore.WebAPI.Tests.Controllers
{
    public class PublisherControllerTests
    {
        [Fact]
        public async Task Get_ValidCall_ReturnsPublishers()
        {

            var mock = AutoMock.GetLoose();
            var data = new List<PublisherBasicDto>(){new PublisherBasicDto(){Name = "DC Comics"}, new PublisherBasicDto(){Name = "Marvel Comics"}};
            mock.Mock<IPublisherService>().Setup(x => x.PublisherListAsync())
                .Returns(Task.FromResult(data.AsEnumerable()));

            var controller = mock.Create<PublisherController>();
            var result = await controller.Get();

            Assert.NotNull(result);
            Assert.Equal(data.Count(),result.Count());
        }

        [Fact]
        public async Task GetById_ValidCall_ReturnsPublisher()
        {

            var mock = AutoMock.GetLoose();
            var publisher = new PublisherDetailsDto(){Name = "DC Comics", Description = "Test desc"};
            mock.Mock<IPublisherService>().Setup(x => x.PublisherDetailsAsync(1)).Returns(Task.FromResult(publisher));

            var controller = mock.Create<PublisherController>();
            var result = await controller.GetById(1).ConfigureAwait(true);

            mock.Mock<IPublisherService>().Verify(x => x.PublisherDetailsAsync(1), Times.Once);
	    var resultPublisher = Assert.IsType<PublisherDetailsDto>(result.Value);
            Assert.Equal(publisher.Name, resultPublisher.Name);

        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            var mock = AutoMock.GetLoose();

            var controller = mock.Create<PublisherController>();
            var result = await controller.GetById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_ValidCall_ReturnsCreatedResult()
        {
            var mock = AutoMock.GetLoose();
            var publisher = new PublisherDto(){Name = "DC Comics", Description = "Description"};

            var controller = mock.Create<PublisherController>();
            var result = await controller.Post(publisher);

	    Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Post_InvalidPublisher_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var publisher = new PublisherDto();
            mock.Mock<IPublisherService>().Setup(x => x.AddPublisherAsync(publisher))
                .Throws(new ValidationException());

            var controller = mock.Create<PublisherController>();
            var result = await controller.Post(publisher);

            var error = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ValidCall_ReturnsNoContentResult()
        {
            var mock = AutoMock.GetLoose();

            var controller = mock.Create<PublisherController>();
            var result = await controller.Delete(1);

            mock.Mock<IPublisherService>().Verify(x => x.DeletePublisherAsync(1), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            mock.Mock<IPublisherService>().Setup(x => x.DeletePublisherAsync(1)).Throws<NullReferenceException>();

            var controller = mock.Create<PublisherController>();
            var result = await controller.Delete(1);

	    Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_ValidCall_ReturnsCreatedResult()
        {
            var mock = AutoMock.GetLoose();
            var publisher = new PublisherDto();

            var controller = mock.Create<PublisherController>();
            var result = await controller.Put(1, publisher).ConfigureAwait(true);
            
	    Assert.IsType<CreatedResult>(result);
            mock.Mock<IPublisherService>().Verify(x => x.UpdatePublisherAsync(1, publisher), Times.Once);
        }

        [Fact]
        public async Task Put_InvalidId_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<PublisherController>();
            var publisher = new PublisherDto();
            mock.Mock<IPublisherService>().Setup(x => x.UpdatePublisherAsync(1, publisher))
                .Throws<NullReferenceException>();

           var result = await controller.Put(1, publisher);

           Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_InvalidPublisher_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var publisher = new PublisherDto();
            mock.Mock<IPublisherService>().Setup(x => x.UpdatePublisherAsync(1, publisher))
                .Throws<ValidationException>();
            var controller = mock.Create<PublisherController>();

            var result = await controller.Put(1, publisher);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Patch_ValidCall()
        {
            var mock = AutoMock.GetLoose();
	    var publisher = new PublisherDto();
            mock.Mock<IPublisherService>().Setup(x => x.PublisherToEditAsync(1)).Returns(Task.FromResult(publisher));
            var controller = mock.Create<PublisherController>();

            var result = await controller.Patch(1, new JsonPatchDocument<PublisherDto>());
            
	    mock.Mock<IPublisherService>().Verify(x => x.UpdatePublisherAsync(1,publisher));
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Patch_PublisherIsNull_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();
            var controller = mock.Create<PublisherController>();

            var result = await controller.Patch(1, new JsonPatchDocument<PublisherDto>());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Patch_UpdateThrowsException_ReturnsBadRequestObjectResult()
        {
            var mock = AutoMock.GetLoose();
	    var publisher = new PublisherDto();
            mock.Mock<IPublisherService>().Setup(x => x.PublisherToEditAsync(1)).Returns(Task.FromResult(publisher));
            mock.Mock<IPublisherService>().Setup(x => x.UpdatePublisherAsync(1, publisher))
                .Throws(new ValidationException("message"));

            var controller = mock.Create<PublisherController>();

            var result = await controller.Patch(1, new JsonPatchDocument<PublisherDto>());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
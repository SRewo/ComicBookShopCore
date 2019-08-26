using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
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
            var data = TestData.GetPublishersSample();
            mock.Mock<IAsyncRepository<Publisher>>().Setup(x => x.GetAllAsync()).Returns(Task.FromResult(data.AsEnumerable()));

            var controller = mock.Create<PublisherController>();
            var result = await controller.Get();

            mock.Mock<IAsyncRepository<Publisher>>().Verify(x => x.GetAllAsync(), Times.Once);
            Assert.Equal(data.Count(),result.Count());

        }

        [Fact]
        public async Task GetById_ValidCall_ReturnsPublisher()
        {

            var mock = AutoMock.GetLoose();
            var publisher = TestData.GetPublishersSample().First();
            mock.Mock<IAsyncRepository<Publisher>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult(publisher));

            var controller = mock.Create<PublisherController>();
            var result = await controller.GetById(1);

	    mock.Mock<IAsyncRepository<Publisher>>().Verify(x => x.GetByIdAsync(1),Times.Once);
	    var resultPublisher = Assert.IsType<Publisher>(result.Value);
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
            var publisher = TestData.GetPublishersSample().First();

            var controller = mock.Create<PublisherController>();
            var result = await controller.Post(publisher);

	    mock.Mock<IAsyncRepository<Publisher>>().Verify(x => x.AddAsync(publisher), Times.Once);
	    Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Post_InvalidPublisher_ReturnsBadRequestResult()
        {
            var mock = AutoMock.GetLoose();
            var publisher = TestData.GetPublishersSample().First();
            publisher.Name = "";

            var controller = mock.Create<PublisherController>();
            var result = await controller.Post(publisher);

	    mock.Mock<IAsyncRepository<Publisher>>().Verify(x => x.AddAsync(publisher), Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ValidCall_ReturnsNoContentResult()
        {
            var mock = AutoMock.GetLoose();
            var publisher = TestData.GetPublishersSample().First();
            mock.Mock<IAsyncRepository<Publisher>>().Setup(x => x.GetByIdAsync(1)).Returns(Task.FromResult(publisher));

            var controller = mock.Create<PublisherController>();
            var result = await controller.Delete(1);

	    mock.Mock<IAsyncRepository<Publisher>>().Verify(x => x.DeleteAsync(publisher),Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsNotFoundResult()
        {
            var mock = AutoMock.GetLoose();

            var controller = mock.Create<PublisherController>();
            var result = await controller.Delete(1);

	    Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ValidCall_ReturnsCreatedResult()
        {
            var mock = AutoMock.GetLoose();
            var publisher = TestData.GetPublishersSample().First();
	    
        }
    }
}
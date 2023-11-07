using Xunit;
using Moq;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using BenefitsCalculator.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using System.Net;
using Moq.Protected;
using System.Net.Http.Json;

namespace BenefitsCalculator.Tests.Controller
{
    public class SetupTests
    {
        private Mock<ILogger<SetupController>> _mockLogger;
        private Mock<IHttpClientFactory> _mockClientFactory;

        public SetupTests()
        {
            _mockLogger = new Mock<ILogger<SetupController>>();
            _mockClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public void Create_RedirectsToIndex_WhenSaveIsSuccessful()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            SetupController controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);
            var setup = new SetupDTO
            {
                MaxAgeLimit = 10,
                MinAgeLimit = 10,
                MaxRange = 10,
                MinRange = 10
            };

            // Act
            var result = controller.Create(setup);

            //Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_ModelStateIsInvalid_WhenMaxAgeLimitIsLessThanMinAgeLimit()
        {
            // Arrange
            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);
            var setup = new SetupDTO
            {
                MaxAgeLimit = 10,
                MinAgeLimit = 11,
            };

            // Act
            var result = controller.Create(setup);

            // Assert
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public void Create_ModelStateIsInvalid_WhenMaxRangeIsLessThanMinRange()
        {
            // Arrange
            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);
            var setup = new SetupDTO
            {
                MaxRange = 10,
                MinRange = 11,
            };

            // Act
            var result = controller.Create(setup);

            // Assert
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public void Details_RedirectsToError_WhenSetupDataIsNotFound()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.Details(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Delete_RedirectsToError_WhenSetupDataIsNotFound()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Delete_RedirectsToIndex_WhenSetupIsSuccessfullyDeleted()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Edit_RedirectsToError_WhenSetupDataIsNotFound()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Edit_ModelStateIsInvalid_WhenMaxAgeLimitIsLessThanMinAgeLimit()
        {
            // Arrange
            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);
            var setup = new SetupDTO
            {
                MaxAgeLimit = 10,
                MinAgeLimit = 11,
            };

            // Act
            var result = controller.Edit(setup);

            // Assert
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public void Edit_ModelStateIsInvalid_WhenMaxRangeIsLessThanMinRange()
        {
            // Arrange
            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);
            var setup = new SetupDTO
            {
                MaxRange = 10,
                MinRange = 11,
            };

            // Act
            var result = controller.Edit(setup);

            // Assert
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public void Edit_ReturnsToIndex_WhenSaveIsSuccessful()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new SetupController(_mockLogger.Object, _mockClientFactory.Object);
            var setup = new SetupDTO();

            // Act
            var result = controller.Edit(setup);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
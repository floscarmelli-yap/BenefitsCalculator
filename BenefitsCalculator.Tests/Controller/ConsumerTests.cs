using AutoMapper;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BenefitsCalculator.Tests.Controller
{
    public class ConsumerTests
    {
        private Mock<ILogger<ConsumerController>> _mockLogger;
        private Mock<IHttpClientFactory> _mockClientFactory;

        public ConsumerTests()
        {
            _mockLogger = new Mock<ILogger<ConsumerController>>();
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

            var controller = new ConsumerController(_mockLogger.Object, _mockClientFactory.Object);
            var consumer = new ConsumerWithSetupIdsDTO();

            // Act
            var result = controller.Create(consumer);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_ModelStateIsInvalid()
        {
            // Arrange
            var controller = new ConsumerController(_mockLogger.Object, _mockClientFactory.Object);
            var consumer = new ConsumerWithSetupIdsDTO();

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.Create(consumer);

            // Assert
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public void Edit_RedirectsToError_WhenConsumerDataIsNotFound()
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

            var controller = new ConsumerController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Edit_ModelStateIsInvalid()
        {
            // Arrange
            var controller = new ConsumerController(_mockLogger.Object, _mockClientFactory.Object);
            var consumer = new ConsumerWithSetupIdsDTO();

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.Edit(consumer);

            // Assert
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public void Delete_RedirectsToError_WhenConsumerDataIsNotFound()
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

            var controller = new ConsumerController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Delete_RedirectsToIndex_WhenConsumerIsSuccessfullyDeleted()
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

            var controller = new ConsumerController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}

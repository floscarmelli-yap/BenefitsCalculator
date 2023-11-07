using AutoMapper;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
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
    public class BenefitsHistoryTests
    {
        private Mock<ILogger<BenefitsHistoryController>> _mockLogger;
        private Mock<IHttpClientFactory> _mockClientFactory;

        public BenefitsHistoryTests()
        {
            _mockLogger = new Mock<ILogger<BenefitsHistoryController>>();
            _mockClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public void Delete_RedirectsToError_WhenBenefitsHistoryDataIsNotFound()
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

            var controller = new BenefitsHistoryController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Delete_RedirectsToIndex_WhenBenefitsHistoryIsSuccessfullyDeleted()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("1")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            var controller = new BenefitsHistoryController(_mockLogger.Object, _mockClientFactory.Object);

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}

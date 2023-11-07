using AutoMapper;
using BenefitsCalculator.ComputationLogic;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Migrations;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BenefitsCalculator.Tests.Controller
{
    public class ComputeBenefitsTests
    {
        private Mock<ILogger<ComputeBenefitsController>> _mockLogger;
        private Mock<IHttpClientFactory> _mockClientFactory;

        public ComputeBenefitsTests()
        {
            _mockLogger = new Mock<ILogger<ComputeBenefitsController>>();
            _mockClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public void ComputationDetails_RedirectsToError_WhenComputationFails()
        {
            // Arrange
            var controller = new ComputeBenefitsController(_mockLogger.Object,
                _mockClientFactory.Object);

            // Act

            // model passed to the BenefitsComputation class will be null;
            // resulting to an exception
            var result = controller.ComputationDetails(It.IsAny<ConsumerSetupDTO>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }
    }
}

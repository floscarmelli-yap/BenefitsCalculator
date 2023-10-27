using AutoMapper;
using BenefitsCalculator.ComputationLogic;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Migrations;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BenefitsCalculator.Tests
{
    public class ComputeBenefitsTests
    {
        private Mock<ILogger<ComputeBenefitsController>> _mockLogger;
        private Mock<IBenefitsRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<UserManager<AppUser>> _mockUserManager;

        public ComputeBenefitsTests()
        {
            _mockLogger = new Mock<ILogger<ComputeBenefitsController>>();
            _mockRepository = new Mock<IBenefitsRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void ComputationDetails_RedirectsToError_WhenComputationFails()
        {
            // Arrange
            var controller = new ComputeBenefitsController(_mockRepository.Object, _mockLogger.Object, 
                _mockMapper.Object, _mockUserManager.Object);

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

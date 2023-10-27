using AutoMapper;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data;
using BenefitsCalculator.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BenefitsCalculator.Tests
{
    public class BenefitsHistoryTests
    {
        private Mock<ILogger<BenefitsHistoryController>> _mockLogger;
        private Mock<IBenefitsRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;


        public BenefitsHistoryTests()
        {
            _mockLogger = new Mock<ILogger<BenefitsHistoryController>>();
            _mockRepository = new Mock<IBenefitsRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockMapper.SetupAllProperties();
            _mockRepository.SetupAllProperties();
            _mockMapper.SetupAllProperties();
        }

        [Fact]
        public void Delete_RedirectsToError_WhenBenefitsHistoryDataIsNotFound()
        {
            // Arrange
            var controller = new BenefitsHistoryController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetBenefitsHistGroupForDelete(It.IsAny<int>()))
                .Returns(Task.FromResult(It.IsAny<BenefitsHistGroup>()));

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
            var controller = new BenefitsHistoryController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetBenefitsHistGroupForDelete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BenefitsHistGroup { Id = 1 }));
            _mockRepository.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}

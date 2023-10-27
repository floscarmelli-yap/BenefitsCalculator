using Xunit;
using Moq;
using BenefitsCalculator.Data;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Mvc;

namespace BenefitsCalculator.Tests
{
    public class SetupTests
    {
        private Mock<ILogger<SetupController>> _mockLogger;
        private Mock<IBenefitsRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;


        public SetupTests()
        {
            _mockLogger = new Mock<ILogger<SetupController>>();
            _mockRepository = new Mock<IBenefitsRepository>();
            _mockMapper = new Mock<IMapper>();

            _mockMapper.SetupAllProperties();
            _mockRepository.SetupAllProperties();
            _mockMapper.SetupAllProperties();
        }

        [Fact]
        public void Create_RedirectsToIndex_WhenSaveIsSuccessful()
        {
            // Arrange
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));
            var setup = new SetupDTO
            {
                MaxAgeLimit = 10,
                MinAgeLimit = 10,
                MaxRange = 10,
                MinRange = 10
            };

            // Act
            var result = controller.Create(setup);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_AddEntityNeverExecutes_WhenMaxAgeLimitIsLessThanMinAgeLimit()
        {
            // Arrange
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var setup = new SetupDTO
            {
                MaxAgeLimit = 10,
                MinAgeLimit = 11,
            };

            // Act
            var result = controller.Create(setup);

            // Assert
            _mockRepository.Verify(x => x.AddEntity(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void Create_AddEntityNeverExecutes_WhenMaxRangeIsLessThanMinRange()
        {
            // Arrange
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var setup = new SetupDTO
            {
                MaxRange = 10,
                MinRange = 11,
            };

            // Act
            var result = controller.Create(setup);

            // Assert
            _mockRepository.Verify(x => x.AddEntity(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void Details_RedirectsToError_WhenSetupDataIsNotFound()
        {
            // Arrange
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetSetupById(It.IsAny<int>())).Returns(Task.FromResult((Setup?)null));

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
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetSetupById(It.IsAny<int>())).Returns(Task.FromResult((Setup?)null));

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
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetSetupById(It.IsAny<int>())).Returns(Task.FromResult((Setup?)new Setup { Id = 1}));
            _mockRepository.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

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
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetSetupById(It.IsAny<int>())).Returns(Task.FromResult((Setup?)null));

            // Act
            var result = controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Edit_UpdateEntityNeverExecutes_WhenMaxAgeLimitIsLessThanMinAgeLimit()
        {
            // Arrange
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var setup = new SetupDTO
            {
                MaxAgeLimit = 10,
                MinAgeLimit = 11,
            };

            // Act
            var result = controller.Edit(setup);

            // Assert
            _mockRepository.Verify(x => x.UpdateEntity(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void Edit_UpdateEntityNeverExecutes_WhenMaxRangeIsLessThanMinRange()
        {
            // Arrange
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var setup = new SetupDTO
            {
                MaxRange = 10,
                MinRange = 11,
            };

            // Act
            var result = controller.Edit(setup);

            // Assert
            _mockRepository.Verify(x => x.UpdateEntity(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void Edit_ReturnsToIndex_WhenSaveIsSuccessful()
        {
            // Arrange
            var controller = new SetupController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var setup = new SetupDTO();

            _mockRepository.Setup(x => x.SetupExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockRepository.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));
            _mockMapper.Setup(x => x.Map<object>(It.IsAny<object>())).Returns(new Setup { Id = 1 });

            // Act
            var result = controller.Edit(setup);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}
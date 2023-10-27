using AutoMapper;
using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
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
    public class ConsumerTests
    {
        private Mock<ILogger<ConsumerController>> _mockLogger;
        private Mock<IBenefitsRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;

        public ConsumerTests()
        {
            _mockLogger = new Mock<ILogger<ConsumerController>>();
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
            var controller = new ConsumerController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var consumer = new ConsumerDTO();
            _mockRepository.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var result = controller.Create(consumer);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_AddEntityNeverExecutes_ModelStateIsInvalid()
        {
            // Arrange
            var controller = new ConsumerController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var consumer = new ConsumerDTO();

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.Create(consumer);

            // Assert
            _mockRepository.Verify(x => x.AddEntity(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void Edit_RedirectsToError_WhenConsumerDataIsNotFound()
        {
            // Arrange
            var controller = new ConsumerController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetConsumerById(It.IsAny<int>())).Returns(Task.FromResult((Consumer?)null));

            // Act
            var result = controller.Edit(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Error", viewResult.ActionName);
        }

        [Fact]
        public void Edit_UpdateEntityNeverExecutes_WhenModelStateIsInvalid()
        {
            // Arrange
            var controller = new ConsumerController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            var consumer = new ConsumerDTO();

            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.Edit(consumer);

            // Assert
            _mockRepository.Verify(x => x.UpdateEntity(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public void Delete_RedirectsToError_WhenConsumerDataIsNotFound()
        {
            // Arrange
            var controller = new ConsumerController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetConsumerById(It.IsAny<int>())).Returns(Task.FromResult((Consumer?)null));

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
            var controller = new ConsumerController(_mockRepository.Object, _mockLogger.Object, _mockMapper.Object);
            _mockRepository.Setup(x => x.GetConsumerById(It.IsAny<int>())).Returns(Task.FromResult((Consumer?)new Consumer { Id = 1 }));
            _mockRepository.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var result = controller.DeleteConfirmed(It.IsAny<int>());

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}

using AutoMapper;
using BenefitsCalculator.API;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
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

namespace BenefitsCalculator.Tests.API
{
    public class ConsumerApiTests
    {
        private Mock<ILogger<ConsumerApi>> _mockLogger;
        private Mock<IConsumerRepository> _mockConsumerRepo;
        private Mock<ICommonRepository> _mockCommonRepo;
        private Mock<IMapper> _mockMapper;

        public ConsumerApiTests() 
        {
            _mockCommonRepo = new Mock<ICommonRepository>();
            _mockConsumerRepo = new Mock<IConsumerRepository>();
            _mockLogger = new Mock<ILogger<ConsumerApi>>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public void Get_ReturnsOk_WhenThereIsNoException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            // Act
            var response = mockConsumerApi.Get();

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetAllConsumers()).Throws(new ArgumentException());

            // Act
            var response = mockConsumerApi.Get();

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk_WhenConsumerDataIsNotNull()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetConsumerById(It.IsAny<int>())).Returns(Task.FromResult((Consumer?)new Consumer()));

            // Act
            var response = mockConsumerApi.Get(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenConsumerDataIsNull()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            // Act
            var response = mockConsumerApi.Get(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(response.Result.Result);
        }

        [Fact]
        public void GetById_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetConsumerById(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockConsumerApi.Get(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void GetIncludingSetup_ReturnsOk_WhenThereIsNoException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetConsumersIncludingSetupById(It.IsAny<int>()))
                .Returns(Task.FromResult(new Consumer { Setup = new Setup() }));

            // Act
            var response = mockConsumerApi.GetIncludingSetup(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void GetIncludingSetup_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetConsumersIncludingSetupById(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockConsumerApi.GetIncludingSetup(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenSuccessfullyCreated()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockConsumerApi.Post(It.IsAny<ConsumerDTO>());

            // Assert
            Assert.IsAssignableFrom<OkResult>(response.Result);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockCommonRepo.Setup(x => x.SaveAll()).Throws(new ArgumentException());

            // Act
            var response = mockConsumerApi.Post(It.IsAny<ConsumerDTO>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result);
        }

        [Fact]
        public void Put_ReturnsOk_WhenConsumerDataIsNotNullAndIsSuccessfullyUpdated()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockMapper.Setup(x => x.Map<Consumer>(It.IsAny<ConsumerDTO>())).Returns(new Consumer { Id = 1});
            _mockConsumerRepo.Setup(x => x.ConsumerExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockConsumerApi.Put(It.IsAny<ConsumerDTO>());

            // Assert
            Assert.IsAssignableFrom<OkResult>(response.Result);
        }

        [Fact]
        public void Put_ReturnsNotFound_WhenConsumerDataDoesNotExist()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockMapper.Setup(x => x.Map<Consumer>(It.IsAny<ConsumerDTO>())).Returns(new Consumer { Id = 1 });
            _mockConsumerRepo.Setup(x => x.ConsumerExists(It.IsAny<int>())).Returns(Task.FromResult(false));

            // Act
            var response = mockConsumerApi.Put(It.IsAny<ConsumerDTO>());

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockMapper.Setup(x => x.Map<Consumer>(It.IsAny<ConsumerDTO>())).Returns(new Consumer { Id = 1 });
            _mockConsumerRepo.Setup(x => x.ConsumerExists(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockConsumerApi.Put(It.IsAny<ConsumerDTO>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenConsumerDataIsNotNullAndIsSuccessfullyDeleted()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetConsumerById(It.IsAny<int>())).Returns(Task.FromResult((Consumer?) new Consumer { Id = 1 }));
            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockConsumerApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkResult>(response.Result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenConsumerDataIsNull()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetConsumerById(It.IsAny<int>())).Returns(Task.FromResult((Consumer?) null));

            // Act
            var response = mockConsumerApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Fact]
        public void Delete_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockConsumerApi = new ConsumerApi(_mockConsumerRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockConsumerRepo.Setup(x => x.GetConsumerById(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockConsumerApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result);
        }
    }
}

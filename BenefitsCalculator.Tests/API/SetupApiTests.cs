using AutoMapper;
using BenefitsCalculator.API;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xunit;

namespace BenefitsCalculator.Tests.API
{
    public class SetupApiTests
    {
        private Mock<ILogger<SetupApi>> _mockLogger;
        private Mock<ISetupRepository> _mockSetupRepo;
        private Mock<ICommonRepository> _mockCommonRepo;
        private Mock<IMapper> _mockMapper;

        public SetupApiTests() 
        {
            _mockCommonRepo = new Mock<ICommonRepository>();
            _mockSetupRepo = new Mock<ISetupRepository>();
            _mockLogger = new Mock<ILogger<SetupApi>>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public void Get_ReturnsOk_WhenThereIsNoException()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);
            
            // Act
            var response = mockSetupApi.Get();

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockSetupRepo.Setup(x => x.GetAllSetup()).Throws(new ArgumentException());

            // Act
            var response = mockSetupApi.Get();

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void GetById_ReturnsOk_WhenSetupDataIsNotNull()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockSetupRepo.Setup(x => x.GetSetupById(It.IsAny<int>())).Returns(Task.FromResult((Setup?) new Setup()));

            // Act
            var response = mockSetupApi.Get(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void GetById_ReturnsNotFound_WhenSetupDataIsNull()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            // Act
            var response = mockSetupApi.Get(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(response.Result.Result);
        }

        [Fact]
        public void GetById_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockSetupRepo.Setup(x => x.GetSetupById(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockSetupApi.Get(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenSuccessfullyCreated()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockSetupApi.Post(It.IsAny<SetupDTO>());

            // Assert
            Assert.IsAssignableFrom<OkResult>(response.Result);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockCommonRepo.Setup(x => x.SaveAll()).Throws(new ArgumentException());

            // Act
            var response = mockSetupApi.Post(It.IsAny<SetupDTO>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result);
        }

        [Fact]
        public void Put_ReturnsOk_WhenSuccessfullyUpdated()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockMapper.Setup(x => x.Map<Setup>(It.IsAny<SetupDTO>())).Returns(new Setup { Id = 1});
            _mockSetupRepo.Setup(x => x.SetupExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockSetupApi.Put(It.IsAny<SetupDTO>());

            // Assert
            Assert.IsAssignableFrom<OkResult>(response.Result);
        }

        [Fact]
        public void Put_ReturnsNotFound_WhenSetupDoesNotExist()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockMapper.Setup(x => x.Map<Setup>(It.IsAny<SetupDTO>())).Returns(new Setup { Id = 1 });
            _mockSetupRepo.Setup(x => x.SetupExists(It.IsAny<int>())).Returns(Task.FromResult(false));

            // Act
            var response = mockSetupApi.Put(It.IsAny<SetupDTO>());

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Fact]
        public void Put_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockMapper.Setup(x => x.Map<Setup>(It.IsAny<SetupDTO>())).Returns(new Setup { Id = 1 });
            _mockSetupRepo.Setup(x => x.SetupExists(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockSetupApi.Put(It.IsAny<SetupDTO>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenSuccessfullyDeleted()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockSetupRepo.Setup(x => x.GetSetupById(It.IsAny<int>())).Returns(Task.FromResult((Setup?) new Setup()));
            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockSetupApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkResult>(response.Result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenSetupDoesNotExist()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockSetupRepo.Setup(x => x.GetSetupById(It.IsAny<int>())).Returns(Task.FromResult((Setup?) null));

            // Act
            var response = mockSetupApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Fact]
        public void Delete_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockSetupApi = new SetupApi(_mockSetupRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, _mockMapper.Object);

            _mockSetupRepo.Setup(x => x.GetSetupById(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockSetupApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result);
        }
    }
}

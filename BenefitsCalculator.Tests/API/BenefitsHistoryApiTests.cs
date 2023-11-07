using AutoMapper;
using BenefitsCalculator.API;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Data.Repositories;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Identity;
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
    public class BenefitsHistoryApiTests
    {
        private Mock<ILogger<BenefitsHistoryApi>> _mockLogger;
        private Mock<IBenefitsHistoryRepository> _mockBenefitsHistRepo;
        private Mock<ICommonRepository> _mockCommonRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<UserManager<AppUser>> _mockUserManager;

        public BenefitsHistoryApiTests() 
        {
            _mockBenefitsHistRepo = new Mock<IBenefitsHistoryRepository>();
            _mockCommonRepo = new Mock<ICommonRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<BenefitsHistoryApi>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void Get_ReturnsOk_WhenThereIsNoException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object, 
                _mockMapper.Object, _mockUserManager.Object);

            // Act
            var response = mockBenefitsHistApi.Get();

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void Get_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockBenefitsHistRepo.Setup(x => x.GetAllBenefitsHistGroups()).Throws(new ArgumentException());

            // Act
            var response = mockBenefitsHistApi.Get();

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void GetConsumerHistories_ReturnsOk_WhenThereIsNoException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            // Act
            var response = mockBenefitsHistApi.GetConsumerHistories(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void GetConsumerHistories_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockBenefitsHistRepo.Setup(x => x.GetConsumerBenefitsHistGroups(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockBenefitsHistApi.GetConsumerHistories(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void GetHistoryDetails_ReturnsOk_WhenThereIsNoException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockBenefitsHistRepo.Setup(x => x.GetBenefitsHistGroupById(It.IsAny<int>()))
                .Returns(Task.FromResult(new BenefitsHistGroup {
                    Consumer = new Consumer(),
                    BenefitsHistories = new List<BenefitsHistory>()}));

            // Act
            var response = mockBenefitsHistApi.GetHistoryDetails(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void GetHistoryDetails_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockBenefitsHistRepo.Setup(x => x.GetBenefitsHistGroupById(It.IsAny<int>())).Throws(new ArgumentException());

            // Act
            var response = mockBenefitsHistApi.GetHistoryDetails(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void Post_ReturnsOk_WhenSuccessfullyCreated()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockMapper.Setup(x => x.Map<BenefitsHistGroup>(It.IsAny<HistGroupDTO>())).Returns(new BenefitsHistGroup { Id = 1 });
            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockBenefitsHistApi.Post(new HistGroupDTO());

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void Post_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockCommonRepo.Setup(x => x.SaveAll()).Throws(new ArgumentException());

            // Act
            var response = mockBenefitsHistApi.Post(new HistGroupDTO());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }

        [Fact]
        public void Delete_ReturnsOk_WhenSuccessfullyDeleted()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockBenefitsHistRepo.Setup(x => x.GetBenefitsHistGroupForDelete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BenefitsHistGroup { Id = 1 }));
            _mockCommonRepo.Setup(x => x.SaveAll()).Returns(Task.FromResult(true));

            // Act
            var response = mockBenefitsHistApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(response.Result.Result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenSetupDoesNotExist()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            // Act
            var response = mockBenefitsHistApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(response.Result.Result);
        }

        [Fact]
        public void Delete_ReturnsBadRequest_WhenThereIsAnException()
        {
            // Arrange
            var mockBenefitsHistApi = new BenefitsHistoryApi(_mockBenefitsHistRepo.Object,
                _mockCommonRepo.Object, _mockLogger.Object,
                _mockMapper.Object, _mockUserManager.Object);

            _mockBenefitsHistRepo.Setup(x => x.GetBenefitsHistGroupForDelete(It.IsAny<int>()))
                .Throws(new ArgumentException());

            // Act
            var response = mockBenefitsHistApi.Delete(It.IsAny<int>());

            // Assert
            Assert.IsAssignableFrom<BadRequestResult>(response.Result.Result);
        }
    }
}

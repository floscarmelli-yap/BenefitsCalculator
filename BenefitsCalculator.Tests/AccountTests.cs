using BenefitsCalculator.Controllers;
using BenefitsCalculator.Data.Entities;
using BenefitsCalculator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BenefitsCalculator.Tests
{
    public class AccountTests
    {
        private Mock<ILogger<AccountController>> _mockLogger;
        private Mock<SignInManager<AppUser>> _mockSignInManager;

        public AccountTests()
        {
            _mockLogger = new Mock<ILogger<AccountController>>();

            var userManagerMock = new Mock<UserManager<AppUser>>(
                Mock.Of<IUserStore<AppUser>>(),
                null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<AppUser>>(
                userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null, null, null, null);
        }

        [Fact]
        public void Login_ReturnsToAppIndex_WhenSigningInSucceeded()
        {
            // Arrange
            var controller = new AccountController(_mockLogger.Object, _mockSignInManager.Object);
            var login = new LoginDTO();
            _mockSignInManager.Setup(x => x
                .PasswordSignInAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            // Act
            var result = controller.Login(login);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result.Result);

            Assert.Equal("App", viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerOnboarding.Api.Controllers;
using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerOnboarding.Tests
{
    public class BanksControllerTests
    {
        private readonly Mock<IBankService> _bankServiceMock;
        private readonly BanksController _controller;

        public BanksControllerTests()
        {
            _bankServiceMock = new Mock<IBankService>();

            // Inject the mock into controller
            _controller = new BanksController(_bankServiceMock.Object);
        }

        [Fact]
        public async Task GetBanks_ReturnsOkResult_WithBankList()
        {
            // Arrange
            var fakeBanks = new List<BankDto>
            {
                new BankDto { Code = "001", Name = "Mock Bank 1" },
                new BankDto { Code = "002", Name = "Mock Bank 2" }
            };

            _bankServiceMock.Setup(s => s.GetBanksAsync())
                .ReturnsAsync(fakeBanks);

            // Act
            var result = await _controller.GetBanks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBanks = Assert.IsAssignableFrom<IEnumerable<BankDto>>(okResult.Value);

            Assert.Collection(returnedBanks,
                bank => Assert.Equal("Mock Bank 1", bank.Name),
                bank => Assert.Equal("Mock Bank 2", bank.Name)
            );

            _bankServiceMock.Verify(s => s.GetBanksAsync(), Times.Once);
        }

        [Fact]
        public async Task GetBanks_ReturnsServerError_WhenServiceFails()
        {
            // Arrange
            _bankServiceMock.Setup(s => s.GetBanksAsync())
                .ThrowsAsync(new System.Exception("Service unavailable"));

            // Act
            var result = await _controller.GetBanks();

            // Assert
            var objResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objResult.StatusCode);
            Assert.Contains("Service unavailable", objResult.Value?.ToString());
        }
    }
}

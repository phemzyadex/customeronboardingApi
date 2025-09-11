using CustomerOnboarding.Api.Controllers;
using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CustomerOnboarding.Tests.Controllers
{
    public class BanksControllerTests
    {
        private readonly Mock<IBankService> _mockBankService;
        private readonly BanksController _controller;

        public BanksControllerTests()
        {
            _mockBankService = new Mock<IBankService>();
            _controller = new BanksController(_mockBankService.Object);
        }

        [Fact]
        public async Task GetBanks_ReturnsOk_WithListOfBanks()
        {
            // Arrange
            var bankResponse = new BankApiResponse
            {
                HasError = false,
                Result = new List<BankDto>
                {
                    new BankDto { Name = "Wema Bank", Code = "035" },
                    new BankDto { Name = "GTBank", Code = "058" }
                },
                TimeGenerated = DateTime.UtcNow
            };

            _mockBankService.Setup(s => s.GetBanksAsync())
                .ReturnsAsync(bankResponse);

            // Act
            var result = await _controller.GetBanks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResponse = Assert.IsType<BankApiResponse>(okResult.Value);

            Assert.False(returnedResponse.HasError);
            Assert.Collection(returnedResponse.Result,
                b => Assert.Equal("Wema Bank", b.Name),
                b => Assert.Equal("GTBank", b.Name));
        }

        [Fact]
        public async Task GetBanks_WhenServiceThrows_Returns500()
        {
            // Arrange
            _mockBankService.Setup(s => s.GetBanksAsync())
                .ThrowsAsync(new Exception("Service unavailable"));

            // Act
            var result = await _controller.GetBanks();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Contains("Service unavailable", objectResult.Value?.ToString());
        }
    }
}

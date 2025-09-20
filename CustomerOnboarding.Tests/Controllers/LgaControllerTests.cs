using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerOnboarding.Api.Controllers;
using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerOnboarding.Tests.Controllers
{
    public class LgaControllerTests
    {
        private readonly Mock<ILgaService> _mockService;
        private readonly LgaController _controller;

        public LgaControllerTests()
        {
            _mockService = new Mock<ILgaService>();
            _controller = new LgaController(_mockService.Object);
        }

        [Fact]
        public async Task AddLga_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var dto = new LgaDto { StateId = Guid.NewGuid(), Name = "Test LGA" };
            var response = new LgaResponseDto { Id = Guid.NewGuid(), Name = "Test LGA", StateName = "Test State" };

            _mockService.Setup(s => s.AddLgaAsync(dto)).ReturnsAsync(response);

            // Act
            var result = await _controller.AddLga(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<LgaResponseDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("LGA created successfully.", apiResponse.Message);
            Assert.Equal("Test LGA", apiResponse.Data!.Name);
        }

        [Fact]
        public async Task AddLga_ShouldReturnBadRequest_WhenDuplicateOrInvalid()
        {
            // Arrange
            var dto = new LgaDto { StateId = Guid.NewGuid(), Name = "Duplicate LGA" };

            _mockService.Setup(s => s.AddLgaAsync(dto))
                        .ThrowsAsync(new InvalidOperationException("LGA already exists."));

            // Act
            var result = await _controller.AddLga(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("LGA already exists.", apiResponse.Message);
        }

        [Fact]
        public async Task GetLgasByState_ShouldReturnOk_WhenLgasExist()
        {
            // Arrange
            var stateId = Guid.NewGuid();
            var lgas = new List<LgaResponseDto>
            {
                new LgaResponseDto { Id = Guid.NewGuid(), Name = "LGA 1", StateName = "Test State" },
                new LgaResponseDto { Id = Guid.NewGuid(), Name = "LGA 2", StateName = "Test State" }
            };

            _mockService.Setup(s => s.GetLgasByStateAsync(stateId)).ReturnsAsync(lgas);

            // Act
            var result = await _controller.GetLgasByState(stateId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<LgaResponseDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.NotEmpty(apiResponse.Data!);
        }

        [Fact]
        public async Task GetLgasByState_ShouldReturnNotFound_WhenNoLgasExist()
        {
            // Arrange
            var stateId = Guid.NewGuid();

            _mockService.Setup(s => s.GetLgasByStateAsync(stateId))
                        .ReturnsAsync(new List<LgaResponseDto>()); // Empty list

            // Act
            var result = await _controller.GetLgasByState(stateId);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFound.Value);
            Assert.False(apiResponse.Success);
            Assert.Equal("No LGAs found for the given state.", apiResponse.Message);
        }
    }
}

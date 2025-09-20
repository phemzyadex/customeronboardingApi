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
    public class StateControllerTests
    {
        private readonly Mock<IStateService> _mockService;
        private readonly StateController _controller;

        public StateControllerTests()
        {
            _mockService = new Mock<IStateService>();
            _controller = new StateController(_mockService.Object);
        }

        [Fact]
        public async Task AddState_ShouldReturnOk_WhenStateCreated()
        {
            // Arrange
            var dto = new StateDto { Name = "Lagos" };
            var response = new StateResponseDto { Id = Guid.NewGuid(), Name = "Lagos" };

            _mockService.Setup(s => s.AddStateAsync(dto.Name))
                        .ReturnsAsync(response);

            // Act
            var result = await _controller.AddState(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<StateDto>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Equal("Lagos", apiResponse.Data.Name);
        }

        [Fact]
        public async Task AddState_ShouldReturnBadRequest_WhenDuplicateState()
        {
            // Arrange
            var dto = new StateDto { Name = "Lagos" };
            _mockService.Setup(s => s.AddStateAsync(dto.Name))
                        .ThrowsAsync(new InvalidOperationException("State 'Lagos' already exists."));

            // Act
            var result = await _controller.AddState(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(badRequest.Value);
            Assert.False(apiResponse.Success);
            Assert.Contains("already exists", apiResponse.Message);
        }

        [Fact]
        public async Task GetAllStates_ShouldReturnOk_WithListOfStates()
        {
            // Arrange
            var states = new List<StateResponseDto>
            {
                new StateResponseDto { Id = Guid.NewGuid(), Name = "Lagos", Lgas = new List<LgaResponseOnly>() },
                new StateResponseDto { Id = Guid.NewGuid(), Name = "Abuja", Lgas = new List<LgaResponseOnly>() }
            };

            _mockService.Setup(s => s.GetAllStatesAsync())
                        .ReturnsAsync(states);

            // Act
            var result = await _controller.GetAllStates();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<StateResponseDto>>>(okResult.Value);
            Assert.True(apiResponse.Success);
            Assert.Collection(apiResponse.Data,
                s => Assert.Equal("Lagos", s.Name),
                s => Assert.Equal("Abuja", s.Name));
        }
    }
}

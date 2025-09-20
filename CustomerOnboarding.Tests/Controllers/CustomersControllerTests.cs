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
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new CustomersController(_mockService.Object);
        }

        [Fact]
        public async Task Onboard_ReturnsAccepted_WhenSuccess()
        {
            var dto = new OnboardCustomerDto { Email = "test@mail.com" };
            var response = ApiResponse<CustomerResponse>.Ok(
                new CustomerResponse { Id = Guid.NewGuid(), Email = dto.Email }, 
                "Onboarded");

            _mockService.Setup(s => s.OnboardAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller.Onboard(dto);

            var accepted = Assert.IsType<AcceptedResult>(result);
            Assert.Equal(response, accepted.Value);
        }

        [Fact]
        public async Task Onboard_ReturnsBadRequest_WhenFailure()
        {
            var dto = new OnboardCustomerDto { Email = "fail@mail.com" };
            var response = ApiResponse<CustomerResponse>.Fail("Error", null);

            _mockService.Setup(s => s.OnboardAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller.Onboard(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response, badRequest.Value);
        }


        [Fact]
        public async Task Verify_ReturnsOk_WhenSuccess()
        {
            var dto = new VerifyPhoneDto { PhoneNumber = "123", Otp = "456" };
            var response = ApiResponse<bool>.Ok(true, "Verified");
            _mockService.Setup(s => s.VerifyPhoneAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller.Verify(dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, ok.Value);
        }

        [Fact]
        public async Task Verify_ReturnsBadRequest_WhenFailure()
        {
            var dto = new VerifyPhoneDto { PhoneNumber = "123", Otp = "999" };
            var response = ApiResponse<bool>.Fail("Invalid OTP", null);
            _mockService.Setup(s => s.VerifyPhoneAsync(dto))
                .ReturnsAsync(response);

            var result = await _controller.Verify(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response, badRequest.Value);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithData()
        {
            var customers = new List<CustomerDto> { new CustomerDto { Email = "c1@mail.com" } };
            var response = ApiResponse<IEnumerable<CustomerDto>>.Ok(customers, "All customers");
            _mockService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(response);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, ok.Value);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsOk_WhenSuccess()
        {
            var id = Guid.NewGuid();
            var response = ApiResponse<bool>.Ok(true, "Deleted");
            _mockService.Setup(s => s.SoftDeleteCustomerAsync(id))
                .ReturnsAsync(response);

            var result = await _controller.DeleteCustomer(id);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(response, ok.Value);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsBadRequest_WhenFailure()
        {
            var id = Guid.NewGuid();
            var response = ApiResponse<bool>.Fail("Not found", null);
            _mockService.Setup(s => s.SoftDeleteCustomerAsync(id))
                .ReturnsAsync(response);

            var result = await _controller.DeleteCustomer(id);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(response, badRequest.Value);
        }
    }
}

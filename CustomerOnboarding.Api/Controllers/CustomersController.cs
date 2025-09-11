using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _svc;
        public CustomersController(ICustomerService svc) => _svc = svc;

        /// <summary>
        /// Onboards a new customer by capturing email, phone, state, and LGA.
        /// Generates an OTP and sends it (mocked).
        /// </summary>
        [HttpPost("onboard")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 202)]
        [ProducesResponseType(typeof(ApiResponse<Guid>), 400)]
        public async Task<IActionResult> Onboard([FromBody] OnboardCustomerDto dto)
        {
            var result = await _svc.OnboardAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Accepted(result);
        }

        /// <summary>
        /// Verifies a customer's phone number using OTP.
        /// </summary>
        [HttpPost("verify-phone")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponse<bool>), 400)]
        public async Task<IActionResult> Verify([FromBody] VerifyPhoneDto dto)
        {
            var result = await _svc.VerifyPhoneAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all onboarded customers.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerDto>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _svc.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Soft deletes a customer by Id (marks as deleted without removing).
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _svc.SoftDeleteCustomerAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    [Produces("application/json")]
    public class StateController : ControllerBase
    {
        private readonly StateService _service;

        public StateController(StateService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adds a new state.
        /// </summary>
        /// <param name="dto">The state details.</param>
        /// <returns>Details of the created state.</returns>
        /// <response code="200">State successfully created.</response>
        /// <response code="400">Validation error (e.g., duplicate state).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StateDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> AddState([FromBody] StateDto dto)
        {
            try
            {
                var state = await _service.AddStateAsync(dto.Name);
                return Ok(ApiResponse<StateDto>.Ok(
                    new StateDto { Name = state.Name },
                    "State created successfully"
                ));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message, null));
            }
        }

        /// <summary>
        /// Retrieves all states.
        /// </summary>
        /// <returns>List of states.</returns>
        /// <response code="200">Returns all states.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<StateDto>>), 200)]
        public async Task<IActionResult> GetAllStates()
        {
            var states = await _service.GetAllStatesAsync();
            return Ok(ApiResponse<IEnumerable<StateResponseDto>>.Ok(states, "States retrieved successfully"));
        }
    }
}

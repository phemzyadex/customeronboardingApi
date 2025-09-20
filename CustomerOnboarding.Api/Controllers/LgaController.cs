using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    [Produces("application/json")]
    public class LgaController : ControllerBase
    {
        private readonly ILgaService _service;

        public LgaController(ILgaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adds a new LGA mapped to a State.
        /// </summary>
        /// <param name="dto">The LGA details including StateId and Name.</param>
        /// <returns>The created LGA with State name.</returns>
        /// <response code="200">LGA created successfully.</response>
        /// <response code="400">Validation failed (e.g. invalid state, duplicate LGA).</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<LgaDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> AddLga([FromBody] LgaDto dto)
        {
            try
            {
                var lga = await _service.AddLgaAsync(dto);
                return Ok(ApiResponse<LgaResponseDto>.Ok(lga, "LGA created successfully."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message, null));
            }
        }

        /// <summary>
        /// Gets all LGAs mapped to a given State.
        /// </summary>
        /// <param name="stateId">The State Id.</param>
        /// <returns>List of LGAs for the given State.</returns>
        /// <response code="200">List of LGAs returned successfully.</response>
        /// <response code="404">State not found.</response>
        [HttpGet("state/{stateId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LgaDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<IActionResult> GetLgasByState(Guid stateId)
        {
            var lgas = await _service.GetLgasByStateAsync(stateId);
            if (lgas == null || !lgas.Any())
                return NotFound(ApiResponse<string>.Fail("No LGAs found for the given state.", null));

            return Ok(ApiResponse<IEnumerable<LgaResponseDto>>.Ok(lgas, null));
        }
    }
}

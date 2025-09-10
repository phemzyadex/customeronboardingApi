using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class LgaController : ControllerBase
    {
        private readonly LgaService _service;

        public LgaController(LgaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddLga([FromBody] LgaDto dto)
        {
            var lga = await _service.AddLgaAsync(dto);
            return Ok(lga);
        }

        [HttpGet("state/{stateId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLgasByState(Guid stateId)
        {
            var lgas = await _service.GetLgasByStateAsync(stateId);
            return Ok(lgas);
        }
    }
}

using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly StateService _service;

        public StateController(StateService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddState([FromBody] StateDto dto)
        {
            var state = await _service.AddStateAsync(dto.Name);
            return Ok(state);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllStates()
        {
            var states = await _service.GetAllStatesAsync();
            return Ok(states);
        }
    }
}

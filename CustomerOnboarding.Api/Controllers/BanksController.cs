using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BanksController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BanksController(IBankService bankService)
        {
            _bankService = bankService;
        }

        /// <summary>
        /// Returns all available banks from ALAT API.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BankDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBanks()
        {
            try
            {
                var banks = await _bankService.GetBanksAsync();
                return Ok(banks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching banks: {ex.Message}");
            }
        }
    }
}

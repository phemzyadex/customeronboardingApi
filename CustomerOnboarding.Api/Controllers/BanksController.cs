// using Microsoft.AspNetCore.Mvc;

// namespace CustomerOnboarding.Api.Controllers
// {
    // [ApiController]
    // [Route("api/[controller]")]
    // public class BanksController : ControllerBase
    // {
        // private readonly IHttpClientFactory _httpFactory;
        // private readonly IConfiguration _config;
        // public BanksController(IHttpClientFactory httpFactory, IConfiguration config)
        // {
            // _httpFactory = httpFactory;
            // _config = config;
        // }

        // [HttpGet]
        // public async Task<IActionResult> GetBanks()
        // {
            // var client = _httpFactory.CreateClient("alat");
            // // endpoint path shown in ALAT docs: /merchant-onboarding/api/v1/banks
            // var res = await client.GetAsync("merchant-onboarding/api/v1/banks");
            // if (!res.IsSuccessStatusCode) return StatusCode((int)res.StatusCode, "Failed to get banks from provider");
            // var body = await res.Content.ReadAsStringAsync();
            // // simply return raw JSON from provider (or map to DTO)
            // return Content(body, "application/json");
        // }
    // }

// }
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

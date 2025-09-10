using CustomerOnboarding.Core.DTOs.Requests;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(AppUser), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(request.Username, request.Password, request.Role);
            if (user == null)
                return BadRequest(new { message = "Username already exists" });

            return CreatedAtAction(nameof(Register), new { id = user.Id }, new { user.Id, user.Username, user.Role });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request.Username, request.Password);
            if (response == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(response);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users.Select(u => new { u.Id, u.Username, u.Role }));
        }
    }
}

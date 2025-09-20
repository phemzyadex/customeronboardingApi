using CustomerOnboarding.Core.DTOs.Requests;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Interfaces;
using CustomerOnboarding.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOnboarding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user (Admin-only).
        /// </summary>
        /// <remarks>
        /// Requires the `Administrator` role.
        /// </remarks>
        /// <param name="request">Username, password, and role for the new user.</param>
        /// <returns>Created user with Id, Username, and Role.</returns>
        /// <response code="201">User registered successfully.</response>
        /// <response code="400">Validation error (e.g., username already exists).</response>
        [HttpPost("register")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<AppUser>), 201)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(request.Username, request.Password, request.Role);
            if (user == null)
                return BadRequest(ApiResponse<string>.Fail("Username already exists", null));

            return CreatedAtAction(nameof(Register), new { id = user.Id },
                ApiResponse<object>.Ok(new { user.Id, user.Username, user.Role }, "User created successfully"));
        }

        /// <summary>
        /// Logs in a user with username and password.
        /// </summary>
        /// <param name="request">Login credentials.</param>
        /// <returns>JWT token with username, role, and expiration.</returns>
        /// <response code="200">Login successful, token returned.</response>
        /// <response code="401">Invalid username or password.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request.Username, request.Password);
            if (response == null)
                return Unauthorized(ApiResponse<string>.Fail("Invalid username or password", null));

            return Ok(ApiResponse<AuthResponseDto>.Ok(response, "Login successful"));
        }

        /// <summary>
        /// Returns all registered users (Admin-only).
        /// </summary>
        /// <remarks>
        /// Requires the `Administrator` role.
        /// </remarks>
        /// <returns>List of users with Id, Username, and Role.</returns>
        /// <response code="200">Users retrieved successfully.</response>
        [HttpGet("users")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<object>>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            var result = users.Select(u => new { u.Id, u.Username, u.Role });
            return Ok(ApiResponse<IEnumerable<object>>.Ok(result, "Users retrieved successfully"));
        }
    }
}

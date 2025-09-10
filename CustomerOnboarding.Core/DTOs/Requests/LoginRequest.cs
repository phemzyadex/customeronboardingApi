namespace CustomerOnboarding.Core.DTOs.Requests
{
    /// <summary>
    /// Request model for logging in a user.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Username of the registered user.
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// Password of the registered user.
        /// </summary>
        public string Password { get; set; } = null!;
    }
}

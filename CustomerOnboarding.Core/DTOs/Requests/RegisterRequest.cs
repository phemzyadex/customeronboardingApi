namespace CustomerOnboarding.Core.DTOs.Requests
{
    /// <summary>
    /// Request model for registering a new application user.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Desired username for the account (must be unique).
        /// </summary>
        public string Username { get; set; } = null!;

        /// <summary>
        /// User's password (will be securely hashed before storing).
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Role assigned to the user (e.g., "Administrator", "User").
        /// </summary>
        public string Role { get; set; } = "User";
    }
}

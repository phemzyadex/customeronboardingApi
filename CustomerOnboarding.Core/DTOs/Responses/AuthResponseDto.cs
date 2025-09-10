namespace CustomerOnboarding.Core.DTOs.Responses
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime Expires { get; set; }
    }
}

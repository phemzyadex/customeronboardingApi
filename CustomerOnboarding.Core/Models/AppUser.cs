namespace CustomerOnboarding.Core.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User";
    }
}

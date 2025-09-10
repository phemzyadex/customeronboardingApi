using CustomerOnboarding.Core.Models;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool IsPhoneVerified { get; set; } = false;
    public bool OnboardingCompleted { get; set; } = false;

    // Foreign key
    public Guid StateId { get; set; }

    // Navigation
    public State State { get; set; } = null!;

    // LGA
    public Guid LgaId { get; set; }
    public Lga Lga { get; set; } = null!;

    public string? PendingOtp { get; set; }
    public DateTime? OtpExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}

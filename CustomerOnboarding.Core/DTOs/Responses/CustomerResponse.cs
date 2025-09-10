using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Core.DTOs.Responses
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public bool IsPhoneVerified { get; set; }

        public bool OnboardingCompleted { get; set; }

        // Location
        public Guid StateId { get; set; }
        public string StateName { get; set; } = null!;

        public Guid LgaId { get; set; }
        public string LgaName { get; set; } = null!;

        // OTP Info (exposed only because it's mock)
        public string? PendingOtp { get; set; }
        public DateTime? OtpExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

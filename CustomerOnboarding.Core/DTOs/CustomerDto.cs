namespace CustomerOnboarding.Core.DTOs
{
    /// <summary>
    /// Response model representing an onboarded customer.
    /// </summary>
    public class CustomerDto
    {
        /// <summary>
        /// Unique identifier for the customer.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Customer's email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Customer's phone number.
        /// </summary>
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// Whether the customer's phone number has been verified via OTP.
        /// </summary>
        public bool IsPhoneVerified { get; set; }

        /// <summary>
        /// Whether the onboarding process has been fully completed.
        /// </summary>
        public bool OnboardingCompleted { get; set; }

        /// <summary>
        /// Customer's state of residence.
        /// </summary>
        public string State { get; set; } = null!;

        /// <summary>
        /// Customer's Local Government Area (LGA).
        /// </summary>
        public string Lga { get; set; } = null!;
    }
}

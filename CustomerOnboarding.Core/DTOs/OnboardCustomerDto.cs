namespace CustomerOnboarding.Core.DTOs
{
    /// <summary>
    /// Request model for onboarding a new customer.
    /// </summary>
    public class OnboardCustomerDto
    {
        /// <summary>
        /// Customer email address (must be unique).
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Customer phone number in international format (e.g., +2348012345678).
        /// </summary>
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// The identifier of the customer's state of residence.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// The identifier of the customer's Local Government Area (LGA).
        /// Must belong to the specified state.
        /// </summary>
        public Guid LgaId { get; set; }

        /// <summary>
        /// Customer Password.
        /// </summary>
        public string Password { get; set; } = null!;
    }
}

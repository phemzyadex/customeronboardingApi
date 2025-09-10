namespace CustomerOnboarding.Core.DTOs
{
    /// <summary>
    /// Request model for verifying a customer's phone number using OTP.
    /// </summary>
    public class VerifyPhoneDto
    {  
        /// <summary>
       /// The customer ID returned after onboarding.
       /// </summary>
        public Guid CustomerId { get; set; }
        /// <summary>
        /// Customer phone number (must match the one used during onboarding).
        /// </summary>
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// One-Time Password (OTP) sent to the customer's phone.
        /// </summary>
        public string Otp { get; set; } = null!;
    }
}

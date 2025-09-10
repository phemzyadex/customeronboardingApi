using CustomerOnboarding.Core.DTOs;
using FluentValidation;

namespace CustomerOnboarding.Core.Validators
{
    public class VerifyPhoneDtoValidator : AbstractValidator<VerifyPhoneDto>
    {
        public VerifyPhoneDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required.")
                .Length(6).WithMessage("OTP must be 6 digits.");
        }
    }
}

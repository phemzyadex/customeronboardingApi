using CustomerOnboarding.Core.DTOs;
using FluentValidation;

namespace CustomerOnboarding.Core.Validators
{
    public class OnboardCustomerDtoValidator : AbstractValidator<OnboardCustomerDto>
    {
        public OnboardCustomerDtoValidator()
        {
            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+[1-9]\d{6,14}$")
            .WithMessage("Phone number must be in international format (e.g., +2348069419299,+14155552671, +442071838750).");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.StateId)
            .NotEmpty().WithMessage("State is required.");


            RuleFor(x => x.LgaId)
            .NotEmpty().WithMessage("LGA is required.");
        }
    }
}
using CustomerOnboarding.Core.DTOs;
using FluentValidation;

namespace CustomerOnboarding.Core.Validators
{
    public class CustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CustomerDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.Lga).NotEmpty();
        }
    }
}

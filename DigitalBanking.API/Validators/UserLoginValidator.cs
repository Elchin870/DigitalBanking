using DigitalBanking.Core.Dtos;
using FluentValidation;

namespace DigitalBanking.API.Validators;

public sealed class UserLoginValidator:AbstractValidator<LoginDto>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required!")
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\w\s]).+$");
    }
}

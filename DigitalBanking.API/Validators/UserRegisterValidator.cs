using DigitalBanking.Core.Dtos;
using FluentValidation;

namespace DigitalBanking.API.Validators;

public sealed class UserRegisterValidator:AbstractValidator<RegisterDto>
{
    public UserRegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required!")
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required!")
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\w\s]).+$")
            .WithMessage("Password must contain...");

        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Firstname is required!")
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Lastname is required!")
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adress is required!")
            .MinimumLength(3);


        RuleFor(x=>x.Age)
            .NotEmpty().WithMessage("Age is required!")
            .GreaterThanOrEqualTo(18).WithMessage("Your age must be at least 18!");
            

    }
}

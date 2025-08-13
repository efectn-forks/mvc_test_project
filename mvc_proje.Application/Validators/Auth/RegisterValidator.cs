using FluentValidation;
using mvc_proje.Application.Dtos.Auth;

namespace mvc_proje.Application.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.PasswordConfirm).NotEmpty().Equal(x => x.Password);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
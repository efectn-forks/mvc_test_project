using FluentValidation;
using mvc_proje.Application.Dtos.Auth;

namespace mvc_proje.Application.Validators.Auth;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty()
            .WithMessage("Kullanıcı adı boş bırakılamaz.");
        RuleFor(x => x.Password).NotEmpty()
            .WithMessage("Parola boş bırakılamaz.");
    }
}
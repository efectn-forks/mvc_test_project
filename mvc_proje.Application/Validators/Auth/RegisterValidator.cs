using FluentValidation;
using mvc_proje.Application.Dtos.Auth;

namespace mvc_proje.Application.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).NotEmpty()
            .WithMessage("Kullanıcı adı boş bırakılamaz.")
            .MaximumLength(50)
            .WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");
        RuleFor(x => x.Password).NotEmpty()
            .WithMessage("Parola boş bırakılamaz.")
            .MinimumLength(6)
            .WithMessage("Parola en az 6 karakter olmalıdır.")
            .MaximumLength(100)
            .WithMessage("Parola 50 karakterden uzun olamaz.");
        RuleFor(x => x.PasswordConfirm).NotEmpty().Equal(x => x.Password)
            .WithMessage("Parola onayı boş bırakılamaz veya parolayla eşleşmiyor.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress()
            .WithMessage("Geçerli bir e-posta adresi girilmelidir.");
    }
}
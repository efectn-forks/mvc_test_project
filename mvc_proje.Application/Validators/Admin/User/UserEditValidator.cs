using FluentValidation;
using mvc_proje.Application.Dtos.Admin.User;

namespace mvc_proje.Application.Validators.Admin.User;

public class UserEditValidator : AbstractValidator<UserEditDto>
{
    public UserEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Username).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Password);
        RuleFor(x => x.PasswordConfirm)
            .Equal(x => x.Password)
            .When(x => !string.IsNullOrEmpty(x.Password));
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(15);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Role).IsInEnum();
        RuleFor(x => x.Avatar).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png");
    }
}
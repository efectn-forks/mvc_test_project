using FluentValidation;
using mvc_proje.Application.Dtos.Profile;

namespace mvc_proje.Application.Validators.Profile;

public class ProfileEditValidator : AbstractValidator<ProfileEditDto>
{
    public ProfileEditValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
        RuleFor(x => x.AvatarUrl).MaximumLength(200);
        RuleFor(x => x.CurrentPassword).Must((dto, currentPassword) =>
            string.IsNullOrEmpty(currentPassword) || !string.IsNullOrEmpty(dto.NewPassword));
        RuleFor(x => x.NewPassword).Must((dto, newPassword) =>
            string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(dto.ConfirmPassword));
        RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).When(x => !string.IsNullOrEmpty(x.NewPassword));
    }
}
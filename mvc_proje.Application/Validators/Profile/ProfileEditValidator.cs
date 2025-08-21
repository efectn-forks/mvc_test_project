using FluentValidation;
using mvc_proje.Application.Dtos.Profile;

namespace mvc_proje.Application.Validators.Profile;

public class ProfileEditValidator : AbstractValidator<ProfileEditDto>
{
    public ProfileEditValidator()
    {
        RuleFor(x => x.FullName).NotEmpty()
            .WithMessage("Ad ve soyad boş bırakılamaz.")
            .MaximumLength(100)
            .WithMessage("Ad ve soyad 100 karakterden uzun olamaz.");
        RuleFor(x => x.Username).NotEmpty()
            .WithMessage("Kullanıcı adı boş bırakılamaz.")
            .MaximumLength(50)
            .WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("E-posta adresi boş bırakılamaz.")
            .EmailAddress()
            .WithMessage("Geçerli bir e-posta adresi girilmelidir.");
        RuleFor(x => x.PhoneNumber).NotEmpty()
            .WithMessage("Telefon numarası boş bırakılamaz.")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Geçerli bir telefon numarası girilmelidir.");
        RuleFor(x => x.Address).NotEmpty()
            .WithMessage("Adres boş bırakılamaz.")
            .MaximumLength(300)
            .WithMessage("Adres 300 karakterden uzun olamaz.");
        RuleFor(x => x.Country).NotEmpty()
            .WithMessage("Ülke boş bırakılamaz.")
            .MaximumLength(50)
            .WithMessage("Ülke 50 karakterden uzun olamaz.");
        RuleFor(x => x.City).NotEmpty()
            .WithMessage("Şehir boş bırakılamaz.")
            .MaximumLength(50)
            .WithMessage("Şehir 50 karakterden uzun olamaz.");
        RuleFor(x => x.ZipCode).NotEmpty()
            .WithMessage("Posta kodu boş bırakılamaz.")
            .MaximumLength(10)
            .WithMessage("Posta kodu 10 karakterden uzun olamaz.");
        RuleFor(x => x.BirthDate).NotEmpty()
            .WithMessage("Doğum tarihi boş bırakılamaz.")
            .LessThan(DateTime.Now)
            .WithMessage("Doğum tarihi gelecekte olamaz.");
        RuleFor(x => x.IdentifyNumber).NotEmpty()
            .WithMessage("Kimlik numarası boş bırakılamaz.")
            .MaximumLength(15)
            .WithMessage("Kimlik numarası 15 karakterden uzun olamaz.");
        RuleFor(x => x.IdentifyNumber).Matches(@"^\d{11}$")
            .WithMessage("Kimlik numarası 11 haneli olmalıdır.");
        RuleFor(x => x.AvatarUrl).MaximumLength(200)
            .WithMessage("Avatar URL'si 200 karakterden uzun olamaz.");
        RuleFor(x => x.CurrentPassword).Must((dto, currentPassword) =>
            string.IsNullOrEmpty(currentPassword) || !string.IsNullOrEmpty(dto.NewPassword));
        RuleFor(x => x.NewPassword).Must((dto, newPassword) =>
            string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(dto.ConfirmPassword));
        RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).When(x => !string.IsNullOrEmpty(x.NewPassword));
    }
}
using FluentValidation;
using mvc_proje.Application.Dtos.Admin.User;

namespace mvc_proje.Application.Validators.Admin.User;

public class UserCreateValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateValidator()
    {
        RuleFor(x => x.Username).NotEmpty()
            .WithMessage("Kullanıcı adı boş bırakılamaz.")
            .MaximumLength(20)
            .WithMessage("Kullanıcı adı 20 karakterden uzun olamaz.");
        RuleFor(x => x.Password).NotEmpty()
            .WithMessage("Parola boş bırakılamaz.")
            .MinimumLength(6)
            .WithMessage("Parola en az 6 karakter olmalıdır.")
            .MaximumLength(50)
            .WithMessage("Parola 50 karakterden uzun olamaz.");
        RuleFor(x => x.PasswordConfirm).NotEmpty()
            .WithMessage("Parola onayı boş bırakılamaz.")
            .Equal(x => x.Password)
            .WithMessage("Parola onayı, parolanızla eşleşmelidir.");
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("E-posta adresi boş bırakılamaz.")
            .EmailAddress()
            .WithMessage("Geçerli bir e-posta adresi girilmelidir.");
        RuleFor(x => x.FullName).NotEmpty()
            .WithMessage("Ad ve soyad boş bırakılamaz.")
            .MaximumLength(50)
            .WithMessage("Ad ve soyad 50 karakterden uzun olamaz.");
        RuleFor(x => x.PhoneNumber).NotEmpty()
            .WithMessage("Telefon numarası boş bırakılamaz.")
            .MaximumLength(15)
            .WithMessage("Telefon numarası 15 karakterden uzun olamaz.");
        RuleFor(x => x.Address).NotEmpty()
            .WithMessage("Adres boş bırakılamaz.")
            .MaximumLength(100)
            .WithMessage("Adres 100 karakterden uzun olamaz.");
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
            .WithMessage("Kimlik numarası 11 haneli sayısal bir değer olmalıdır.");
        RuleFor(x => x.Role).IsInEnum()
            .WithMessage("Geçersiz rol seçimi yapılmıştır. Lütfen geçerli bir rol seçiniz.");
        RuleFor(x => x.Avatar).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .WithMessage("Avatar dosyası boş bırakılamaz veya 5 MB'den büyük olamaz.")
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
            .WithMessage("Avatar dosyası JPEG veya PNG formatında olmalıdır.");
    }
}
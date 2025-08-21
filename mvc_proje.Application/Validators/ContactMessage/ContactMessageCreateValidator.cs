using FluentValidation;
using mvc_proje.Application.Dtos.Admin.ContactMessage;
using mvc_proje.Application.Dtos.ContactMessage;

namespace mvc_proje.Application.Validators.ContactMessage;

public class ContactMessageCreateValidator : AbstractValidator<ContactMessageCreateDto>
{
    public ContactMessageCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Ad boş bırakılamaz.")
            .MaximumLength(100)
            .WithMessage("Ad 100 karakterden uzun olamaz.");
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("E-posta boş bırakılamaz.")
            .EmailAddress()
            .WithMessage("Geçerli bir e-posta adresi girilmelidir.");
        RuleFor(x => x.Subject).NotEmpty()
            .WithMessage("Konuyu boş bırakamaz.")
            .MaximumLength(50)
            .WithMessage("Konu 50 karakterden uzun olamaz.");
        RuleFor(x => x.Message).NotEmpty()
            .WithMessage("Mesaj boş bırakılamaz.")
            .MaximumLength(500)
            .WithMessage("Mesaj 500 karakterden uzun olamaz.");
    }
}
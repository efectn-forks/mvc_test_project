using FluentValidation;
using mvc_proje.Application.Dtos.Admin.ContactMessage;
using mvc_proje.Application.Dtos.ContactMessage;

namespace mvc_proje.Application.Validators.ContactMessage;

public class ContactMessageCreateValidator : AbstractValidator<ContactMessageCreateDto>
{
    public ContactMessageCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(500);
    }
}
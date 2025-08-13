using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Tag;

namespace mvc_proje.Application.Validators.Admin.Tag;

public class TagCreateValidator : AbstractValidator<TagCreateDto>
{
    public TagCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(200);
    }
}
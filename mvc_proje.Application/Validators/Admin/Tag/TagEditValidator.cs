using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Tag;

namespace mvc_proje.Application.Validators.Admin.Tag;

public class TagEditValidator : AbstractValidator<TagEditDto>
{
    public TagEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Slug).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(200);
    }
}
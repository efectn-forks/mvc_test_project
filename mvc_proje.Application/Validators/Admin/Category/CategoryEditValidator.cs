using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Category;

namespace mvc_proje.Application.Validators.Admin.Category;

public class CategoryEditValidator : AbstractValidator<CategoryEditDto>
{
    public CategoryEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Slug).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(255);
    }
}
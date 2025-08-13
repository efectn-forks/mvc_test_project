using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Category;

namespace mvc_proje.Application.Validators.Admin.Category;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(255);
    }
}
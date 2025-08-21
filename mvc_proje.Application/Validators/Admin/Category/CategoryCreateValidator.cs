using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Category;

namespace mvc_proje.Application.Validators.Admin.Category;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori adı boş bırakılamaz.")
            .MaximumLength(50).WithMessage("Kategori adı 50 karakterden uzun olamaz.");
        RuleFor(x => x.Description).
            NotEmpty().WithMessage("Kategori açıklaması boş bırakılamaz.").
            MaximumLength(255).WithMessage("Kategori açıklaması 255 karakterden uzun olamaz.");
    }
}
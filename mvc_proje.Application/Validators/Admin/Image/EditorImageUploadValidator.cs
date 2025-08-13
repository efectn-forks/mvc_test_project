using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace mvc_proje.Application.Validators.Admin.Image;

public class EditorImageUploadValidator : AbstractValidator<IFormFile>
{
    public EditorImageUploadValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .Must(f => f.Length > 0);

        RuleFor(x => x.Length)
            .LessThanOrEqualTo(5 * 1024 * 1024);
    }
}
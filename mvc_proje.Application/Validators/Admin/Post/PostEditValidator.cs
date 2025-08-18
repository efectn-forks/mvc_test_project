using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Post;

namespace mvc_proje.Application.Validators.Admin.Post;

public class PostEditValidator : AbstractValidator<PostEditDto>
{
    public PostEditValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(255);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Image).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png");
    }
}
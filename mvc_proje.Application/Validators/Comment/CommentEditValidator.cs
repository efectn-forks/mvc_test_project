using FluentValidation;
using mvc_proje.Application.Dtos.Comment;

namespace mvc_proje.Application.Validators.Comment;

public class CommentEditValidator : AbstractValidator<CommentEditDto>
{
    public CommentEditValidator()
    {
        RuleFor(c => c.Text).NotEmpty().MaximumLength(500);
        RuleFor(c => c.Id).GreaterThan(0);
    }
}
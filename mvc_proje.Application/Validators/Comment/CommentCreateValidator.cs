using FluentValidation;
using mvc_proje.Application.Dtos.Comment;

namespace mvc_proje.Application.Validators.Comment;

public class CommentCreateValidator : AbstractValidator<CommentCreateDto>
{
    public CommentCreateValidator()
    {
        RuleFor(c => c.Text).NotEmpty().MaximumLength(500);
        RuleFor(c => c.PostId).GreaterThan(0);
        RuleFor(c => c.UserId).GreaterThan(0);
        RuleFor(c => c.ParentId).GreaterThanOrEqualTo(0);
    }
}
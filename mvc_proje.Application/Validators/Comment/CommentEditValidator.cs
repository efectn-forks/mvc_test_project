using FluentValidation;
using mvc_proje.Application.Dtos.Comment;

namespace mvc_proje.Application.Validators.Comment;

public class CommentEditValidator : AbstractValidator<CommentEditDto>
{
    public CommentEditValidator()
    {
        RuleFor(c => c.Text).NotEmpty()
            .WithMessage("Yorum metni boş bırakılamaz.")
            .MaximumLength(500)
            .WithMessage("Yorum metni 500 karakterden uzun olamaz.");
        RuleFor(c => c.Id).GreaterThan(0)
            .WithMessage("Yorum ID'si 0'dan büyük olmalıdır.");
    }
}
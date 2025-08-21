using FluentValidation;
using mvc_proje.Application.Dtos.Comment;

namespace mvc_proje.Application.Validators.Comment;

public class CommentCreateValidator : AbstractValidator<CommentCreateDto>
{
    public CommentCreateValidator()
    {
        RuleFor(c => c.Text).NotEmpty()
            .WithMessage("Yorum metni boş bırakılamaz.")
            .MaximumLength(500)
            .WithMessage("Yorum metni 500 karakterden uzun olamaz.");
        RuleFor(c => c.PostId).GreaterThan(0)
            .WithMessage("Post ID'si 0'dan büyük olmalıdır.");
        RuleFor(c => c.UserId).GreaterThan(0)
            .WithMessage("Kullanıcı ID'si 0'dan büyük olmalıdır.");
        RuleFor(c => c.ParentId).GreaterThanOrEqualTo(0)
            .WithMessage("Parent ID'si 0 veya daha büyük olmalıdır. (0, kök yorum için kullanılır)");
    }
}
using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Post;

namespace mvc_proje.Application.Validators.Admin.Post;

public class PostEditValidator : AbstractValidator<PostEditDto>
{
    public PostEditValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0)
            .WithMessage("Gönderi ID'si boş bırakılamaz ve 0'dan büyük olmalıdır.");
        RuleFor(x => x.Description).MaximumLength(255)
            .WithMessage("Açıklama alanı 255 karakterden uzun olamaz.");
        RuleFor(x => x.Content).NotEmpty()
            .WithMessage("İçerik alanı boş bırakılamaz.");
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0)
            .WithMessage("Kullanıcı ID'si boş bırakılamaz ve 0'dan büyük olmalıdır.");
        RuleFor(x => x.Image).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .WithMessage("Resim dosyası boş bırakılamaz veya 5 MB'den büyük olamaz.")
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
            .WithMessage("Resim dosyası JPEG veya PNG formatında olmalıdır.");
    }
}
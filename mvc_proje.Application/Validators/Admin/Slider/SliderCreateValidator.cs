using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Slider;

namespace mvc_proje.Application.Validators.Admin.Slider;

public class SliderCreateValidator : AbstractValidator<SliderCreateDto>
{
    public SliderCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
            .WithMessage("Başlık boş bırakılamaz.")
            .MaximumLength(100)
            .WithMessage("Başlık 100 karakterden uzun olamaz.");
        RuleFor(x => x.Button1Url).MaximumLength(200)
            .WithMessage("Buton 1 URL'si 200 karakterden uzun olamaz.");
        RuleFor(x => x.Button1Text).MaximumLength(50)
            .WithMessage("Buton 1 metni 50 karakterden uzun olamaz.");
        RuleFor(x => x.Button2Url).MaximumLength(200)
            .WithMessage("Buton 2 URL'si 200 karakterden uzun olamaz.");
        RuleFor(x => x.Button2Text).MaximumLength(50)
            .WithMessage("Buton 2 metni 50 karakterden uzun olamaz.");
        RuleFor(x => x.Image).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .WithMessage("Resim dosyası boş bırakılamaz veya 5 MB'den büyük olamaz.")
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
            .WithMessage("Resim dosyası JPEG veya PNG formatında olmalıdır.");
    }
}
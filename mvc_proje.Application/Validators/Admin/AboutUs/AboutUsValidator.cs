using FluentValidation;
using mvc_proje.Application.Dtos.Admin.AboutUs;

namespace mvc_proje.Application.Validators.Admin.AboutUs;

public class AboutUsValidator : AbstractValidator<AboutUsDto>
{
    public AboutUsValidator()
    {
        RuleFor(x => x.MainTitle).NotEmpty().WithMessage("Başlık alanı boş bırakılamaz.")
            .Length(0, 100).WithMessage("Başlık alanı 100 karakterden uzun olamaz.");
        RuleFor(x => x.MainDescription).NotEmpty().WithMessage("Açıklama alanı boş bırakılamaz.")
            .Length(0, 255).WithMessage("Açıklama alanı 255 karakterden uzun olamaz.");
        RuleFor(x => x.Elements1).NotEmpty().WithMessage("Öğe 1 alanı boş bırakılamaz.")
            .Length(0, 255).WithMessage("Öğe 1 alanı 255 karakterden uzun olamaz.");
        RuleFor(x => x.Elements2).NotEmpty().WithMessage("Öğe 2 alanı boş bırakılamaz.")
            .Length(0, 255).WithMessage("Öğe 2 alanı 255 karakterden uzun olamaz.");
        RuleFor(x => x.Elements3).NotEmpty().WithMessage("Öğe 3 alanı boş bırakılamaz.")
            .Length(0, 255).WithMessage("Öğe 3 alanı 255 karakterden uzun olamaz.");
        RuleFor(x => x.ReadMoreLink).NotEmpty().WithMessage("Daha fazla oku bağlantısı boş bırakılamaz.");
        RuleFor(x => x.Subtitle).NotEmpty().WithMessage("Alt başlık alanı boş bırakılamaz.")
            .Length(0, 100).WithMessage("Alt başlık alanı 100 karakterden uzun olamaz.");
        RuleFor(x => x.SubtitleDescription).NotEmpty().WithMessage("Alt başlık açıklaması alanı boş bırakılamaz.")
            .Length(0, 255).WithMessage("Alt başlık açıklaması alanı 255 karakterden uzun olamaz.");
        RuleFor(x => x.SubtitleLink).NotEmpty().WithMessage("Alt başlık bağlantısı boş bırakılamaz.")
            .Length(0, 100).WithMessage("Alt başlık bağlantısı 100 karakterden uzun olamaz.");
    }
}
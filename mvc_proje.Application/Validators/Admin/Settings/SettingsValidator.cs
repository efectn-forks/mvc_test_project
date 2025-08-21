using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Settings;

namespace mvc_proje.Application.Validators.Admin.Settings;

public class SettingsValidator : AbstractValidator<SettingsDto>
{
    public SettingsValidator()
    {
        RuleFor(x => x.Address).NotEmpty()
            .WithMessage("Adres alanı boş bırakılamaz.")
            .MaximumLength(200)
            .WithMessage("Adres 200 karakterden uzun olamaz.");
        RuleFor(x => x.Phone).NotEmpty()
            .WithMessage("Telefon alanı boş bırakılamaz.")
            .Matches(@"^\+?[0-9\s]+$")
            .WithMessage("Telefon numarası geçerli bir formatta olmalıdır.");
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("E-posta alanı boş bırakılamaz.")
            .EmailAddress()
            .WithMessage("E-posta adresi geçerli bir formatta olmalıdır.");
        RuleFor(x => x.FacebookUrl).NotEmpty()
            .WithMessage("Facebook URL'si boş bırakılamaz.")
            .Must(BeAValidUrl)
            .WithMessage("Facebook URL'si geçerli bir formatta olmalıdır.");
        RuleFor(x => x.TwitterUrl).NotEmpty()
            .WithMessage("Twitter URL'si boş bırakılamaz.")
            .Must(BeAValidUrl)
            .WithMessage("Twitter URL'si geçerli bir formatta olmalıdır.");
        RuleFor(x => x.InstagramUrl).NotEmpty()
            .WithMessage("Instagram URL'si boş bırakılamaz.")
            .Must(BeAValidUrl)
            .WithMessage("Instagram URL'si geçerli bir formatta olmalıdır.");
        RuleFor(x => x.LinkedinUrl).NotEmpty()
            .WithMessage("LinkedIn URL'si boş bırakılamaz.")
            .Must(BeAValidUrl)
            .WithMessage("LinkedIn URL'si geçerli bir formatta olmalıdır.");
        RuleFor(x => x.FooterDescription).NotEmpty()
            .WithMessage("Footer açıklaması boş bırakılamaz.")
            .MaximumLength(500)
            .WithMessage("Footer açıklaması 500 karakterden uzun olamaz.");
        RuleFor(x => x.Sitename).NotEmpty()
            .WithMessage("Site adı boş bırakılamaz.")
            .MaximumLength(100)
            .WithMessage("Site adı 100 karakterden uzun olamaz.");
        RuleFor(x => x.GoogleMapsUrl).NotEmpty()
            .WithMessage("Google Maps URL'si boş bırakılamaz.")
            .Must(BeAValidUrl)
            .WithMessage("Google Maps URL'si geçerli bir formatta olmalıdır.");
    }

    private bool BeAValidUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
}
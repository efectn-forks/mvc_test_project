using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Settings;

namespace mvc_proje.Application.Validators.Admin.Settings;

public class SettingsValidator : AbstractValidator<SettingsDto>
{
    public SettingsValidator()
    {
        RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\+?[0-9\s]+$");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FacebookUrl).NotEmpty().Must(BeAValidUrl);
        RuleFor(x => x.TwitterUrl).NotEmpty().Must(BeAValidUrl);
        RuleFor(x => x.InstagramUrl).NotEmpty().Must(BeAValidUrl);
        RuleFor(x => x.LinkedinUrl).NotEmpty().Must(BeAValidUrl);
        RuleFor(x => x.FooterDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Sitename).NotEmpty().MaximumLength(100);
        RuleFor(x => x.GoogleMapsUrl).NotEmpty().Must(BeAValidUrl);
    }

    private bool BeAValidUrl(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
}
using FluentValidation;
using mvc_proje.Application.Dtos.Admin.AboutUs;

namespace mvc_proje.Application.Validators.Admin.AboutUs;

public class AboutUsValidator : AbstractValidator<AboutUsDto>
{
    public AboutUsValidator()
    {
        RuleFor(x => x.MainTitle).NotEmpty().Length(0, 100);
        RuleFor(x => x.MainDescription).NotEmpty().Length(0, 255);
        RuleFor(x => x.Elements1).NotEmpty().Length(0, 255);
        RuleFor(x => x.Elements2).NotEmpty().Length(0, 255);
        RuleFor(x => x.Elements3).NotEmpty().Length(0, 255);
        RuleFor(x => x.ReadMoreLink).NotEmpty();
        RuleFor(x => x.Subtitle).NotEmpty().Length(0, 100);
        RuleFor(x => x.SubtitleDescription).NotEmpty().Length(0, 255);
        RuleFor(x => x.SubtitleLink).NotEmpty().Length(0, 100);
    }
    
}
using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Slider;

namespace mvc_proje.Application.Validators.Admin.Slider;

public class SliderCreateValidator : AbstractValidator<SliderCreateDto>
{
    public SliderCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Button1Url).MaximumLength(200);
        RuleFor(x => x.Button1Text).MaximumLength(50);
        RuleFor(x => x.Button2Url).MaximumLength(200);
        RuleFor(x => x.Button2Text).MaximumLength(50);
        RuleFor(x => x.Image).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png");
    }
}
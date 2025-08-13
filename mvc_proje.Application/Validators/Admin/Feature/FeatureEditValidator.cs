using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Feature;

namespace mvc_proje.Application.Validators.Admin.Feature;

public class FeatureEditValidator : AbstractValidator<FeatureEditDto>
{
    public FeatureEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Icon).MaximumLength(50);
        RuleFor(x => x.Link).MaximumLength(200);
        RuleFor(x => x.Link).Matches(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$");
    }
}
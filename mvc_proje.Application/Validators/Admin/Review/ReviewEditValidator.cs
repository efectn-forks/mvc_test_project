using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Review;

namespace mvc_proje.Application.Validators.Admin.Review;

public class ReviewEditValidator : AbstractValidator<ReviewEditDto>
{
    public ReviewEditValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Text).NotEmpty().MaximumLength(500);
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.UserTitle).NotEmpty().MaximumLength(100);
    }
}
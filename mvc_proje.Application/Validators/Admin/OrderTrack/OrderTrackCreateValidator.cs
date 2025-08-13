using FluentValidation;
using mvc_proje.Application.Dtos.Admin.OrderTrack;
using mvc_proje.Application.Dtos.Order;

namespace mvc_proje.Application.Validators.Admin.OrderTrack;

public class OrderTrackCreateValidator : AbstractValidator<OrderTrackCreateDto>
{
    public OrderTrackCreateValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.TrackingInfo).NotEmpty().MaximumLength(500);
        RuleFor(x => x.OrderId).GreaterThan(0);
        RuleFor(x => x.CreatedAt).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.Add(TimeSpan.FromHours(5)));
    }
}
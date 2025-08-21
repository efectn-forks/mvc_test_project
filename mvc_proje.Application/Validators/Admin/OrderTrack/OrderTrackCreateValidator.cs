using FluentValidation;
using mvc_proje.Application.Dtos.Admin.OrderTrack;
using mvc_proje.Application.Dtos.Order;

namespace mvc_proje.Application.Validators.Admin.OrderTrack;

public class OrderTrackCreateValidator : AbstractValidator<OrderTrackCreateDto>
{
    public OrderTrackCreateValidator()
    {
        RuleFor(x => x.Status).IsInEnum()
            .WithMessage("Geçersiz sipariş durumu.");
        RuleFor(x => x.TrackingInfo).NotEmpty().MaximumLength(500)
            .WithMessage("Takip bilgisi boş bırakılamaz ve 500 karakterden uzun olamaz.");
        RuleFor(x => x.OrderId).GreaterThan(0)
            .WithMessage("Sipariş ID'si 0'dan büyük olmalıdır.");
        RuleFor(x => x.CreatedAt).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.Add(TimeSpan.FromHours(5)))
            .WithMessage("Oluşturulma tarihi boş bırakılamaz ve şu andan 5 saat ileriyi geçemez.");
    }
}
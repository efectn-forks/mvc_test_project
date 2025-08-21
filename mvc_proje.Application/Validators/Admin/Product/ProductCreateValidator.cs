using FluentValidation;
using mvc_proje.Application.Dtos.Admin.Product;

namespace mvc_proje.Application.Validators.Admin.Product;

public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100)
            .WithMessage("Ürün adı boş bırakılamaz ve 100 karakterden uzun olamaz.");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500)
            .WithMessage("Ürün açıklaması boş bırakılamaz ve 500 karakterden uzun olamaz.");
        RuleFor(x => x.SkuNumber).NotEmpty().MaximumLength(75)
            .WithMessage("SKU numarası boş bırakılamaz ve 75 karakterden uzun olamaz.");
        RuleFor(x => x.Price).NotEmpty().GreaterThan(0)
            .WithMessage("Fiyat alanı boş bırakılamaz ve 0'dan büyük olmalıdır.");
        RuleFor(x => x.CategoryId).NotEmpty().GreaterThan(0)
            .WithMessage("Kategori ID'si boş bırakılamaz ve 0'dan büyük olmalıdır.");
        RuleFor(x => x.Image).Must(file => file == null || (file.Length > 0 && file.Length <= 5 * 1024 * 1024))
            .WithMessage("Resim dosyası boş bırakılamaz veya 5 MB'den büyük olamaz.")
            .Must(file => file == null || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
            .WithMessage("Resim dosyası JPEG veya PNG formatında olmalıdır.");
    }
}
using mvc_proje.Application.Dtos.ProductFeature;
using mvc_proje.Application.Validators.Admin.ProductFeature;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Application.Services.Admin;

public class ProductFeatureService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductFeatureCreateValidator _productFeatureCreateValidator;

    public ProductFeatureService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _productFeatureCreateValidator = new ProductFeatureCreateValidator();
    }

    public async Task<ProductFeatureDto> GetAllAsync()
    {
        var productFeatures = await _unitOfWork.ProductFeatureRepository.GetAllAsync();
        return new ProductFeatureDto { ProductFeatures = productFeatures.ToList() };
    }

    public async Task CreateAsync(ProductFeatureCreateDto productFeature)
    {
        var validationResult = await _productFeatureCreateValidator.ValidateAsync(productFeature);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(
                $"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }
        
        var newProductFeature = new ProductFeature
        {
            ProductId = productFeature.ProductId,
            Key = productFeature.Key,
            Value = productFeature.Value
        };

        await _unitOfWork.ProductFeatureRepository.AddAsync(newProductFeature);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var productFeature = await _unitOfWork.ProductFeatureRepository.GetByIdAsync(id);
        if (productFeature == null)
        {
            throw new KeyNotFoundException($"Product feature with ID {id} not found.");
        }

        await _unitOfWork.ProductFeatureRepository.DeleteAsync(productFeature.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
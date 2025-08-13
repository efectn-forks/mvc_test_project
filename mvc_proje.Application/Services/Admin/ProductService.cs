using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Product;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Admin.Product;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services.Admin;

public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductCreateValidator _productCreateValidator;
    private readonly ProductEditValidator _productEditValidator;

    public ProductService(
        IUnitOfWork unitOfWork,
        ProductCreateValidator productCreateValidator = null,
        ProductEditValidator productEditValidator = null)
    {
        _unitOfWork = unitOfWork;
        _productCreateValidator = productCreateValidator ?? new ProductCreateValidator();
        _productEditValidator = productEditValidator ?? new ProductEditValidator();
    }

    public async Task<ProductDto> GetAllAsync()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync(includeFunc: q => q
            .Include(p => p.Category));

        return new ProductDto
        {
            Products = products
        };
    }

    public async Task CreateAsync(ProductCreateDto model)
    {
        var validationResult = await _productCreateValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            CategoryId = model.CategoryId,
            SkuNumber = model.SkuNumber,
            Stock = model.Stock,
        };

        if (model.Image != null)
        {
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";
            var filePath = Path.Combine("wwwroot", "images", "products", fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            product.ImageUrl = $"/images/products/{fileName}";
        }

        await _unitOfWork.ProductRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ProductEditDto> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(p => p.Category));

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        return new ProductEditDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            SkuNumber = product.SkuNumber,
            ImageUrl = product.ImageUrl,
            Stock = product.Stock
        };
    }
    
    public async Task<Product> GetByIdAsync2(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(p => p.Category));

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }
        
        return product;
    }

    public async Task UpdateAsync(ProductEditDto model)
    {
        var validationResult = await _productEditValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(model.Id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {model.Id} not found.");
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;
        product.SkuNumber = model.SkuNumber;
        product.Stock = model.Stock;

        if (model.Image != null)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldImagePath = Path.Combine("wwwroot", product.ImageUrl.TrimStart('/'));
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Image.FileName)}";
            var filePath = Path.Combine("wwwroot", "images", "products", fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            product.ImageUrl = $"/images/products/{fileName}";
        }

        await _unitOfWork.ProductRepository.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        // Delete image if exists
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            var imagePath = Path.Combine("wwwroot", product.ImageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        await _unitOfWork.ProductRepository.DeleteAsync(product.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetRelatedProducts(int categoryId, int productId)
    {
        return await _unitOfWork.ProductRepository.GetRelatedProductsAsync(categoryId, productId);
    }
}
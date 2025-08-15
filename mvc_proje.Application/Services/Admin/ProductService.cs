using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Product;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Admin.Product;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

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

    public async Task<PagedResult<Product>> GetPagedAsync(int pageNumber)
    {
        var totalProducts = await _unitOfWork.ProductRepository.CountAsync();
        var products = await _unitOfWork.ProductRepository.GetPagedAsync(pageNumber, includeFunc: q => q
            .Include(p => p.Category));

        return new PagedResult<Product>
        {
            Items = products,
            TotalCount = totalProducts,
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
            Content = model.Content,
        };

        var productImages = new List<ProductImage>();
        var i = 0;
        foreach (var image in model.Files)
        {
            if (image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine("wwwroot", "images", "products", fileName);
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                productImages.Add(new ProductImage
                {
                    ImageUrl = $"/images/products/{fileName}",
                    IsMain = i == model.MainImageIndex,
                });
            }

            i++;
        }

        if (productImages.Count > 0)
        {
            product.Images = productImages;
        }

        await _unitOfWork.ProductRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ProductEditDto> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(p => p.Category)
            .Include(p => p.ProductFeatures)
            .Include(p => p.Images));

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        return new ProductEditDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Content = product.Content,
            Price = product.Price,
            CategoryId = product.CategoryId,
            SkuNumber = product.SkuNumber,
            Stock = product.Stock,
            ProductFeatures = product.ProductFeatures.ToList(),
            ProductImages = product.Images.ToList()
        };
    }

    public async Task<Product> GetByIdAsync2(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(p => p.Category)
            .Include(p => p.ProductFeatures)
            .Include(p => p.Images));

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

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(model.Id, includeFunc: q => q
            .Include(p => p.Images));
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {model.Id} not found.");
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Content = model.Content;
        product.Price = model.Price;
        product.CategoryId = model.CategoryId;
        product.SkuNumber = model.SkuNumber;
        product.Stock = model.Stock;

        // delete old images 
        if (model.DeletedImageIds != null && model.DeletedImageIds.Count > 0)
        {
            foreach (var imageId in model.DeletedImageIds)
            {
                var image = product.Images.FirstOrDefault(i => i.Id == imageId);
                if (image != null)
                {
                    // Delete image file
                    var imagePath = Path.Combine("wwwroot", image.ImageUrl.TrimStart('/'));
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }

                    product.Images.Remove(image);
                }
            }
        }

        // add new images
        var newImages = new List<ProductImage>();
        if (model.Files != null && model.Files.Count > 0)
        {
            foreach (var image in model.Files)
            {
                if (image.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var filePath = Path.Combine("wwwroot", "images", "products", fileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    newImages.Add(new ProductImage
                    {
                        ImageUrl = $"/images/products/{fileName}",
                    });
                }
            }
        }

        // Set main image
        foreach (var img in product.Images)
            img.IsMain = false;

        var mainSet = false;

        if (model.MainExistingImageId.HasValue &&
            (model.DeletedImageIds == null || !model.DeletedImageIds.Contains(model.MainExistingImageId.Value)))
        {
            var mainExisting = product.Images.FirstOrDefault(i => i.Id == model.MainExistingImageId.Value);
            if (mainExisting != null)
            {
                mainExisting.IsMain = true;
                mainSet = true;
            }
        }

        if (!mainSet && model.MainNewFileIndex.HasValue)
        {
            var idx = model.MainNewFileIndex.Value;
            if (idx >= 0 && idx < newImages.Count)
            {
                newImages[idx].IsMain = true;
            }
        }

        if (newImages.Count > 0)
        {
            product.Images.AddRange(newImages);
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

        // Delete product images
        foreach (var image in product.Images)
        {
            var imagePath = Path.Combine("wwwroot", image.ImageUrl.TrimStart('/'));
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
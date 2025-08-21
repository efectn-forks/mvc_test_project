using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.ProductOption;
using mvc_proje.Application.Validators.Admin.ProductOption;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services.Admin;

public class ProductOptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductOptionCreateValidator _createValidator;
    private readonly ProductOptionEditValidator _editValidator;

    public ProductOptionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _createValidator = new ProductOptionCreateValidator();
        _editValidator = new ProductOptionEditValidator();
    }

    public async Task AddProductOptionAsync(ProductOptionCreateDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(
                $"Bir hata oluştu: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }

        var productOption = new Domain.Entities.ProductOption
        {
            Name = dto.Name,
        };

        foreach (var value in dto.ValuesSplitted)
        {
            productOption.Values.Add(new Domain.Entities.ProductOptionValue
            {
                Value = value
            });
        }


        await _unitOfWork.ProductOptionRepository.AddAsync(productOption);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ProductOptionDto> GetPagedAsync(int page = 1)
    {
        var totalCount = await _unitOfWork.ProductOptionRepository.CountAsync();
        
        var productOptions = await _unitOfWork.ProductOptionRepository.GetPagedAsync(
            page, 
            includeFunc: q => q.Include(po => po.Values));

        return new ProductOptionDto
        {
            ProductOptions = new Domain.Misc.PagedResult<Domain.Entities.ProductOption>
            {
                Items = productOptions,
                TotalCount = totalCount,
            }
        };
    }
    
    public async Task<ProductOptionEditDto> GetByIdAsync(int id)
    {
        var productOption = await _unitOfWork.ProductOptionRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(po => po.Values));

        if (productOption == null)
        {
            throw new KeyNotFoundException("Ürün seçeneği bulunamadı.");
        }

        return new ProductOptionEditDto
        {
            Id = productOption.Id,
            Name = productOption.Name,
            Values = string.Join(", ", productOption.Values.Select(v => v.Value))
        };
    }
    
    public async Task UpdateProductOptionAsync(ProductOptionEditDto dto)
    {
        var validationResult = await _editValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(
                $"Bir hata oluştu: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }

        var productOption = await _unitOfWork.ProductOptionRepository.GetByIdAsync(dto.Id, includeFunc: q => q
            .Include(po => po.Values));

        if (productOption == null)
        {
            throw new KeyNotFoundException("Ürün seçeneği bulunamadı.");
        }

        productOption.Name = dto.Name;
        productOption.Values.Clear();

        var existingValues = productOption.Values.Select(v => v.Value).ToList();
        productOption.Values.Clear();
        foreach (var value in dto.ValuesSplitted)
        {
            if (!existingValues.Contains(value))
            {
                productOption.Values.Add(new Domain.Entities.ProductOptionValue
                {
                    Value = value
                });
            }
        }

        await _unitOfWork.ProductOptionRepository.UpdateAsync(productOption);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteProductOptionAsync(int id)
    {
        var productOption = await _unitOfWork.ProductOptionRepository.GetByIdAsync(id);
        if (productOption == null)
        {
            throw new KeyNotFoundException("Ürün seçeneği bulunamadı.");
        }

        await _unitOfWork.ProductOptionRepository.DeleteAsync(productOption.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
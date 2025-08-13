using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Category;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Admin.Category;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services.Admin;

public class CategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CategoryCreateValidator _categoryCreateValidator;
    private readonly CategoryEditValidator _categoryUpdateValidator;

    public CategoryService(
        IUnitOfWork unitOfWork,
        CategoryCreateValidator categoryCreateValidator = null,
        CategoryEditValidator categoryUpdateValidator = null)
    {
        _unitOfWork = unitOfWork;
        _categoryCreateValidator = categoryCreateValidator ?? new CategoryCreateValidator();
        _categoryUpdateValidator = categoryUpdateValidator ?? new CategoryEditValidator();
    }

    public async Task<CategoryIndexDto> GetAllAsync()
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync(includeFunc:
            c => c.Include(x => x.Products));
        return new CategoryIndexDto
        {
            Categories = categories,
        };
    }

    public async Task<CategoryEditDto> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        return new CategoryEditDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task CreateAsync(CategoryCreateDto dto)
    {
        var validationResult = await _categoryCreateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed", nameof(dto));
        }

        var category = new Domain.Entities.Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _unitOfWork.CategoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryEditDto dto)
    {
        var validationResult = await _categoryUpdateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed", nameof(dto));
        }

        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.Id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {dto.Id} not found.");
        }

        category.Name = dto.Name;
        category.Description = dto.Description;

        await _unitOfWork.CategoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        await _unitOfWork.CategoryRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
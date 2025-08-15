using mvc_proje.Application.Dtos.Admin.Category;
using mvc_proje.Application.Dtos.Admin.Feature;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Admin.Category;
using mvc_proje.Application.Validators.Admin.Feature;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class FeatureService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly FeatureCreateValidator _featureCreateValidator;
    private readonly FeatureEditValidator _featureEditValidator;

    public FeatureService(
        IUnitOfWork unitOfWork,
        FeatureCreateValidator featureCreateValidator = null,
        FeatureEditValidator featureEditValidator = null)
    {
        _unitOfWork = unitOfWork;
        _featureCreateValidator = featureCreateValidator ?? new FeatureCreateValidator();
        _featureEditValidator = featureEditValidator ?? new FeatureEditValidator();
    }

    public async Task<FeatureDto> GetAllAsync()
    {
        var features = await _unitOfWork.FeatureRepository.GetAllAsync();
        return new FeatureDto
        {
            Features = features,
        };
    }

    public async Task<PagedResult<Feature>> GetPagedAsync(int pageNumber)
    {
        var totalCount = await _unitOfWork.FeatureRepository.CountAsync();
        var features = await _unitOfWork.FeatureRepository.GetPagedAsync(pageNumber);

        return new PagedResult<Feature>
        {
            Items = features,
            TotalCount = totalCount,
        };  
    }

    public async Task<FeatureEditDto> GetByIdAsync(int id)
    {
        var feature = await _unitOfWork.FeatureRepository.GetByIdAsync(id);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with ID {id} not found.");
        }

        return new FeatureEditDto
        {
            Id = feature.Id,
            Title = feature.Title,
            Description = feature.Description,
            Icon = feature.Icon,
            Link = feature.Link
        };
    }
    
    public async Task CreateAsync(FeatureCreateDto dto)
    {
        var validationResult = await _featureCreateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed", nameof(dto));
        }

        var feature = new Domain.Entities.Feature
        {
            Title = dto.Title,
            Description = dto.Description,
            Icon = dto.Icon,
            Link = dto.Link
        };

        await _unitOfWork.FeatureRepository.AddAsync(feature);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(FeatureEditDto dto)
    {
        var validationResult = await _featureEditValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed", nameof(dto));
        }

        var feature = await _unitOfWork.FeatureRepository.GetByIdAsync(dto.Id);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with ID {dto.Id} not found.");
        }

        feature.Title = dto.Title;
        feature.Description = dto.Description;
        feature.Icon = dto.Icon;
        feature.Link = dto.Link;

        await _unitOfWork.FeatureRepository.UpdateAsync(feature);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var feature = await _unitOfWork.FeatureRepository.GetByIdAsync(id);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with ID {id} not found.");
        }

        await _unitOfWork.FeatureRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
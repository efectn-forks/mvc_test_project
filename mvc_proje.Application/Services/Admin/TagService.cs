using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Tag;
using mvc_proje.Application.Validators.Admin.Tag;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services.Admin;

public class TagService
{
    private readonly IUnitOfWork _unitOfWork;
    private TagCreateValidator _tagCreateValidator;
    private TagEditValidator _tagEditValidator;

    public TagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _tagCreateValidator = new TagCreateValidator();
        _tagEditValidator = new TagEditValidator();
    }

    public async Task<TagDto> GetAllAsync()
    {
        var tags = await _unitOfWork.TagRepository.GetAllAsync(includeFunc: q => q
            .Include(t => t.Posts));
        return new TagDto
        {
            Tags = tags.Select(tag => new Tag
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                Posts = tag.Posts
            }),
        };
    }

    public async Task<TagEditDto> GetByIdAsync(int id)
    {
        var tag = await _unitOfWork.TagRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(t => t.Posts));

        if (tag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {id} not found.");
        }

        return new TagEditDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description
        };
    }
    
    public async Task<Tag> GetById2Async(int id)
    {
        var tag = await _unitOfWork.TagRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(t => t.Posts));

        if (tag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {id} not found.");
        }

        return tag;
    }
    
    public async Task CreateAsync(TagCreateDto dto)
    {
        var validationResult = await _tagCreateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var tag = new Tag
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _unitOfWork.TagRepository.AddAsync(tag);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(TagEditDto dto)
    {
        var validationResult = await _tagEditValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var tag = await _unitOfWork.TagRepository.GetByIdAsync(dto.Id);
        if (tag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {dto.Id} not found.");
        }

        tag.Name = dto.Name;
        tag.Description = dto.Description;

        await _unitOfWork.TagRepository.UpdateAsync(tag);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var tag = await _unitOfWork.TagRepository.GetByIdAsync(id);
        if (tag == null)
        {
            throw new KeyNotFoundException($"Tag with ID {id} not found.");
        }

        await _unitOfWork.TagRepository.DeleteAsync(tag.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
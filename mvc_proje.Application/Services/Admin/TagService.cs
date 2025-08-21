using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Tag;
using mvc_proje.Application.Utils;
using mvc_proje.Application.Validators.Admin.Tag;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

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
                Slug = tag.Slug,
                Description = tag.Description,
                Posts = tag.Posts
            }),
        };
    }
    
    public async Task<PagedResult<Tag>> GetPagedAsync(int pageNumber)
    {
        var totalCount = await _unitOfWork.TagRepository.CountAsync();
        var tags = await _unitOfWork.TagRepository.GetPagedAsync(pageNumber, includeFunc: q => q
            .Include(t => t.Posts));

        return new PagedResult<Tag>
        {
            Items = tags,
            TotalCount = totalCount,
        };
    }

    public async Task<TagEditDto> GetByIdAsync(int id)
    {
        var tag = await _unitOfWork.TagRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(t => t.Posts));

        if (tag == null)
        {
            throw new KeyNotFoundException($"{id} ID'li etiket bulunamadı.");
        }

        return new TagEditDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Slug = tag.Slug,
            Description = tag.Description
        };
    }
    
    public async Task<Tag> GetById2Async(int id)
    {
        var tag = await _unitOfWork.TagRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(t => t.Posts)
            .ThenInclude(p => p.User));

        if (tag == null)
        {
            throw new KeyNotFoundException($"{id} ID'li etiket bulunamadı.");
        }

        return tag;
    }
    
    public async Task<Tag> GetBySlugAsync(string slug)
    {
        var tag = await _unitOfWork.TagRepository.GetBySlugAsync(slug, includeFunc: q => q
            .Include(t => t.Posts)
            .ThenInclude(p => p.User));

        if (tag == null)
        {
            throw new KeyNotFoundException($"{slug} slug'lı etiket bulunamadı.");
        }

        return tag;
    }
    
    public async Task CreateAsync(TagCreateDto dto)
    {
        var validationResult = await _tagCreateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Bazı alanlar geçersiz: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var tag = new Tag
        {
            Name = dto.Name,
            Description = dto.Description,
            Slug = dto.Slug,
        };
        
        // generate new unique slug using SlugHelper in case it was not provided
        if (string.IsNullOrWhiteSpace(tag.Slug))
        {
            int i = 0;
            tag.Slug = SlugUtils.Slugify(tag.Name);
            while (await _unitOfWork.PostRepository.SlugExistsAsync(tag.Slug))
            {
                i++;
                tag.Slug += $"-{i}";
            }
        }

        await _unitOfWork.TagRepository.AddAsync(tag);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(TagEditDto dto)
    {
        var validationResult = await _tagEditValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException("Bazı alanlar geçersiz: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var tag = await _unitOfWork.TagRepository.GetByIdAsync(dto.Id);
        if (tag == null)
        {
            throw new KeyNotFoundException($"{dto.Id} ID'li etiket bulunamadı.");
        }

        tag.Name = dto.Name;
        tag.Slug = dto.Slug;
        tag.Description = dto.Description;

        await _unitOfWork.TagRepository.UpdateAsync(tag);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var tag = await _unitOfWork.TagRepository.GetByIdAsync(id);
        if (tag == null)
        {
            throw new KeyNotFoundException($"{id} ID'li etiket bulunamadı.");
        }

        await _unitOfWork.TagRepository.DeleteAsync(tag.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
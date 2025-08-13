using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Post;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Admin.Post;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class PostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PostCreateValidator _postCreateValidator;
    private readonly PostEditValidator _postEditValidator;

    public PostService(
        IUnitOfWork unitOfWork,
        PostCreateValidator postCreateValidator = null,
        PostEditValidator postEditValidator = null)
    {
        _unitOfWork = unitOfWork;
        _postCreateValidator = postCreateValidator ?? new PostCreateValidator();
        _postEditValidator = postEditValidator ?? new PostEditValidator();
    }

    public async Task<PostDto> GetAllAsync()
    {
        var posts = await _unitOfWork.PostRepository.GetAllAsync(includeFunc: q => q
            .Include(p => p.User)
        );

        return new PostDto
        {
            Posts = posts
        };
    }

    public async Task<PagedResult<Post>> GetPagedAsync(int pageNumber)
    {
        var totalPosts = await _unitOfWork.PostRepository.CountAsync();
        var posts = await _unitOfWork.PostRepository.GetPagedAsync(pageNumber, includeFunc: q => q
            .Include(p => p.User)
            .Include(p => p.Tags));

        return new PagedResult<Post>
        {
            Items = posts,
            TotalCount = totalPosts,
        };
    }

    public async Task<PostEditDto> GetByIdAsync(int id)
    {
        var post = await _unitOfWork.PostRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(p => p.User)
            .Include(p => p.Tags));

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        return new PostEditDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            UserId = post.UserId,
            Tags = string.Join(",", post.Tags.Select(t => t.Name)),
            ImageUrl = post.ImageUrl
        };
    }

    public async Task<Post> GetById2Async(int id)
    {
        var post = await _unitOfWork.PostRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(p => p.User)
            .Include(p => p.Tags));

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        return post;
    }

    public async Task<PostEditDto> CreateAsync(PostCreateDto dto)
    {
        var validationResult = await _postCreateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var post = new Post
        {
            Title = dto.Title,
            Description = dto.Description,
            Content = dto.Content,
            UserId = dto.UserId
        };

        await _updateTags(post, dto.Tags);

        if (dto.Image != null && dto.Image.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(dto.Image.FileName)}";
            var imagePath = Path.Combine("wwwroot", "images", "posts", fileName);
            await using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            post.ImageUrl = $"/images/posts/{fileName}";
        }

        await _unitOfWork.PostRepository.AddAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return new PostEditDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            UserId = post.UserId,
            ImageUrl = post.ImageUrl
        };
    }

    public async Task<PostEditDto> UpdateAsync(PostEditDto dto)
    {
        var validationResult = await _postEditValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var post = await _unitOfWork.PostRepository.GetByIdAsync(dto.Id, includeFunc: q => q
            .Include(p => p.User)
            .Include(p => p.Tags));

        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {dto.Id} not found.");
        }

        post.Title = dto.Title;
        post.Description = dto.Description;
        post.Content = dto.Content;
        post.UserId = dto.UserId;

        await _updateTags(post, dto.Tags);

        if (dto.Image != null && dto.Image.Length > 0)
        {
            // Remove the old image if it exists
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                var oldImagePath = Path.Combine("wwwroot", post.ImageUrl.TrimStart('/'));
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }

            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(dto.Image.FileName)}";
            var imagePath = Path.Combine("wwwroot", "images", "posts", fileName);
            await using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            post.ImageUrl = $"/images/posts/{fileName}";
        }

        await _unitOfWork.PostRepository.UpdateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return new PostEditDto
        {
            Id = post.Id,
            Title = post.Title,
            Description = post.Description,
            Content = post.Content,
            UserId = post.UserId,
            ImageUrl = post.ImageUrl
        };
    }

    public async Task DeleteAsync(int id)
    {
        var post = await _unitOfWork.PostRepository.GetByIdAsync(id);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with ID {id} not found.");
        }

        // Remove the image if it exists
        if (!string.IsNullOrEmpty(post.ImageUrl))
        {
            var imagePath = Path.Combine("wwwroot", post.ImageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        await _unitOfWork.PostRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<Post>> GetRecentPostsAsync(int count = 5)
    {
        return await _unitOfWork.PostRepository.GetRecentPostsAsync(count);
    }

    private async Task _updateTags(Post post, string tags)
    {
        post.Tags.Clear();
        if (string.IsNullOrWhiteSpace(tags)) return;

        var tagsSplitted = tags.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var tag in tagsSplitted)
        {
            var existingTag =
                (await _unitOfWork.TagRepository.FindAsync(t => t.Name.ToLower() == tag.Trim().ToLower()))
                .FirstOrDefault();

            if (existingTag != null)
            {
                post.Tags.Add(existingTag);
            }
            else
            {
                var newTag = new Tag { Name = tag.Trim() };
                await _unitOfWork.TagRepository.AddAsync(newTag);
                post.Tags.Add(newTag);
            }
        }
    }
}
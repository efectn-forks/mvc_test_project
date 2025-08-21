using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Review;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Admin.Review;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class ReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ReviewCreateValidator _reviewCreateValidator;
    private readonly ReviewEditValidator _reviewEditValidator;

    public ReviewService(IUnitOfWork unitOfWork,
        ReviewCreateValidator reviewCreateValidator = null,
        ReviewEditValidator reviewEditValidator = null)
    {
        _unitOfWork = unitOfWork;
        _reviewCreateValidator = reviewCreateValidator ?? new ReviewCreateValidator();
        _reviewEditValidator = reviewEditValidator ?? new ReviewEditValidator();
    }

    public async Task<ReviewDto> GetAllAsync()
    {
        var reviews = await _unitOfWork.ReviewRepository.GetAllAsync(includeFunc: q => q.Include(r => r.User));
        return new ReviewDto { Reviews = reviews };
    }
    
    public async Task<PagedResult<Review>> GetPagedAsync(int pageNumber)
    {
        var totalCount = await _unitOfWork.ReviewRepository.CountAsync();
        var reviews = await _unitOfWork.ReviewRepository.GetPagedAsync(pageNumber, 
            includeFunc: q => q.Include(r => r.User));

        return new PagedResult<Review>
        {
            Items = reviews,
            TotalCount = totalCount,
        };
    }

    public async Task CreateAsync(ReviewCreateDto model)
    {
        var validationResult = await _reviewCreateValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Validation failed: " +
                                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var review = new Domain.Entities.Review
        {
            Text = model.Text,
            UserTitle = model.UserTitle,
            UserId = model.UserId
        };

        await _unitOfWork.ReviewRepository.AddAsync(review);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReviewEditDto model)
    {
        var validationResult = await _reviewEditValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Bazı alanlar geçersiz: " +
                                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(model.Id);
        if (review == null)
        {
            throw new KeyNotFoundException("İnceleme bulunamadı.");
        }

        review.Text = model.Text;
        review.UserTitle = model.UserTitle;
        review.UserId = model.UserId;

        await _unitOfWork.ReviewRepository.UpdateAsync(review);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            throw new KeyNotFoundException("İnceleme bulunamadı.");
        }

        await _unitOfWork.ReviewRepository.DeleteAsync(review.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ReviewEditDto> GetByIdAsync(int id)
    {
        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            throw new KeyNotFoundException("İnceleme bulunamadı.");
        }

        return new ReviewEditDto
        {
            Id = review.Id,
            Text = review.Text,
            UserTitle = review.UserTitle,
            UserId = review.UserId
        };
    }
}
using System.Security.Claims;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.ProductReview;
using mvc_proje.Application.Validators.ProductReview;
using mvc_proje.Domain.Enums;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class ProductReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductReviewCreateValidator _createValidator;
    private readonly ProductReviewEditValidator _editValidator;

    public ProductReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _createValidator = new ProductReviewCreateValidator();
        _editValidator = new ProductReviewEditValidator();
    }

    public async Task<bool> CanUserReviewProductAsync(ClaimsPrincipal user, int productId)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            return false;
        }

        var userIdStr = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            return false;
        }

        var user2 = await _unitOfWork.UserRepository.GetByIdAsync(userId, includeFunc: q => q
            .Include(u => u.Orders)
            .ThenInclude(o => o.OrderItems)
            .ThenInclude(oi => oi.Product));
        if (user2 == null)
        {
            throw new InvalidOperationException("Kullanıcı ID'si geçersiz veya kullanıcı bulunamadı.");
        }

        // Check if the user has purchased the product
        return user2.Orders.Any(o =>
            o.PaymentStatus == PaymentStatus.Completed && o.OrderItems.Any(oi => oi.ProductId == productId));
    }

    public async Task CreateReviewAsync(ClaimsPrincipal user, ProductReviewCreateDto reviewDto)
    {
        var validationResult = await _createValidator.ValidateAsync(reviewDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(
                $"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }
        
        var userIdStr = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            throw new InvalidOperationException("Kullanıcı ID'si geçersiz veya kullanıcı bulunamadı.");
        }

        var review = new Domain.Entities.ProductReview
        {
            ProductId = reviewDto.ProductId,
            Rating = reviewDto.Rating,
            Text = reviewDto.Text,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ProductReviewRepository.AddAsync(review);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<ProductReviewEditDto> GetByIdAsync(int reviewId)
    {
        var review = await _unitOfWork.ProductReviewRepository.GetByIdAsync(reviewId, includeFunc: q => q
            .Include(r => r.User)
            .Include(r => r.Product));
        if (review == null)
        {
            throw new KeyNotFoundException("İnceleme bulunamadı");
        }

        return new ProductReviewEditDto
        {
            Id = review.Id,
            Text = review.Text,
            Rating = review.Rating,
            User = review.User,
            Product = review.Product,
            CreatedAt = review.CreatedAt
        };
    }

    public async Task EditReviewAsync(ClaimsPrincipal user, ProductReviewEditDto reviewDto)
    {
        var validationResult = await _editValidator.ValidateAsync(reviewDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(
                $"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }
        
        var review = await _unitOfWork.ProductReviewRepository.GetByIdAsync(reviewDto.Id, includeFunc: q => q
            .Include(r => r.User)
            .Include(r => r.Product));
        if (review == null)
        {
            throw new KeyNotFoundException("İnceleme bulunamadı");
        }

        var userIdStr = user.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            throw new InvalidOperationException("Kullanıcı ID'si geçersiz veya kullanıcı bulunamadı.");
        }

        if (review.UserId != userId)
        {
            throw new UnauthorizedAccessException("Bu incelemeyi düzenleme izniniz yok.");
        }

        review.Rating = reviewDto.Rating;
        review.Text = reviewDto.Text;
        review.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ProductReviewRepository.UpdateAsync(review);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(ClaimsPrincipal? user, int reviewId)
    {
        var review = await _unitOfWork.ProductReviewRepository.GetByIdAsync(reviewId);
        if (review == null)
        {
            throw new KeyNotFoundException("İnceleme bulunamadı");
        }

        var userIdStr = user?.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
        if (!int.TryParse(userIdStr, out var userId) && userIdStr != null)
        {
            throw new InvalidOperationException("Kullanıcı ID'si geçersiz veya kullanıcı bulunamadı.");
        }

        if (review.UserId != userId && user != null)
        {
            throw new UnauthorizedAccessException("Bu incelemeyi silme izniniz yok.");
        }

        await _unitOfWork.ProductReviewRepository.DeleteAsync(review.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
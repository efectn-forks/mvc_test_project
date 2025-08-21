using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Wishlist;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class WishlistService
{
    private readonly IUnitOfWork _unitOfWork;

    public WishlistService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddToWishlistAsync(ClaimsPrincipal user, int productId)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Kullanıcı wishlist'e ürün eklemek için giriş yapmış olmalıdır.");
        }

        var userId = int.Parse(user.FindFirst("UserId")?.Value ??
                               throw new InvalidOperationException("Kullanıcı ID'si bulunamadı."));

        // Check if the product already exists in the wishlist
        var existingWishlistItem = await _unitOfWork.WishlistRepository
            .FindAsync(w => w.UserId == userId && w.ProductId == productId);
        if (existingWishlistItem != null && existingWishlistItem.Any())
        {
            throw new InvalidOperationException("Bu ürün zaten wishlist'inizde mevcut.");
        }

        var wishlistItem = new Wishlist
        {
            UserId = userId,
            ProductId = productId
        };

        await _unitOfWork.WishlistRepository.AddAsync(wishlistItem);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveFromWishlistAsync(ClaimsPrincipal user, int productId)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Kullanıcı wishlist'ten ürün kaldırmak için giriş yapmış olmalıdır.");
        }

        var userId = int.Parse(user.FindFirst("UserId")?.Value ??
                               throw new InvalidOperationException("Kullanıcı ID'si bulunamadı."));

        var wishlistItem = await _unitOfWork.WishlistRepository
            .FindAsync(w => w.UserId == userId && w.ProductId == productId);

        if (wishlistItem == null)
        {
            throw new InvalidOperationException("Wishlist'te böyle bir ürün bulunamadı.");
        }

        await _unitOfWork.WishlistRepository.DeleteAsync(wishlistItem.FirstOrDefault().Id);
        ;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<WishlistDto> GetWishlistAsync(ClaimsPrincipal user)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Kullanıcı wishlist'i görüntülemek için giriş yapmış olmalıdır.");
        }

        var userId = int.Parse(user.FindFirst("UserId")?.Value ??
                               throw new InvalidOperationException("Kullanıcı ID'si bulunamadı."));

        var wishlistItems = await _unitOfWork.WishlistRepository
            .FindAsync(w => w.UserId == userId, includeFunc: query => query
                .Include(w => w.Product)
                .ThenInclude(p => p.StockTransactions)
                .Include(p => p.User));

        var wishlistDto = new WishlistDto 
        {
            WishlistItems = wishlistItems.Select(w => new WishlistElement
            {
                Name = w.Product.Name,
                SkuNumber = w.Product.SkuNumber,
                Stock = w.Product.Stock(),
                Price = w.Product.Price,
                ImageUrl = w.Product.GetMainImage().ImageUrl,
                ProductId = w.Product.Id
            }).ToList()
        };
        
        return wishlistDto;
    }
    
    public async Task ClearWishlistAsync(ClaimsPrincipal user)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Kullanıcı wishlist'i temizlemek için giriş yapmış olmalıdır.");
        }

        var userId = int.Parse(user.FindFirst("UserId")?.Value ??
                               throw new InvalidOperationException("Kullanıcı ID'si bulunamadı."));

        var wishlistItems = await _unitOfWork.WishlistRepository
            .FindAsync(w => w.UserId == userId);

        foreach (var item in wishlistItems)
        {
            await _unitOfWork.WishlistRepository.DeleteAsync(item.Id);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Post;
using mvc_proje.Application.Dtos.Admin.Product;
using mvc_proje.Application.Dtos.Search;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services;

public class SearchService
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<SearchPostDto> SearchPostPagedAsync(string searchTerm, int pageNumber)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("Arama terimi boş olamaz", nameof(searchTerm));
        }

        var totalCount = await _unitOfWork.PostRepository.CountAsync(p =>
            p.Title.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower()));

        var posts = await _unitOfWork.PostRepository.GetPagedAsync(pageNumber, 5,
            p => p.Title.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower()),
            includeFunc: q => q.Include(p => p.User));

        var tags = await _unitOfWork.TagRepository.GetAllAsync();
        var recentPosts = await _unitOfWork.PostRepository.GetRecentPostsAsync(5);

        return new SearchPostDto
        {
            PagedPosts = new PagedResult<Post>
            {
                Items = posts,
                TotalCount = totalCount
            },
            Tags = tags.ToList(),
            RecentPosts = recentPosts.ToList(),
            TotalCount = totalCount,
            SearchTerm = searchTerm
        };
    }
    
    public async Task<ProductDto> SearchProductAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("Arama terimi boş olamaz", nameof(searchTerm));
        }

        var products = await _unitOfWork.ProductRepository.FindAsync(p =>
                p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower()),
            includeFunc: q => q.Include(p => p.Category));

        return new ProductDto
        {
            Products = products.ToList(),
        };
    }
}
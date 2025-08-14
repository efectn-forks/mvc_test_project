using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Post;
using mvc_proje.Application.Dtos.Admin.Product;
using mvc_proje.Application.Dtos.Search;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class SearchService
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SearchPostDto> SearchPostAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new ArgumentException("Search Term cannot be null or empty");
        }

        var post = await _unitOfWork.PostRepository.FindAsync(p =>
                p.Title.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower()),
            includeFunc: q => q.Include(p => p.User));

        var tags = await _unitOfWork.TagRepository.GetAllAsync();
        var recentPosts = await _unitOfWork.PostRepository.GetRecentPostsAsync(5);
        var totalCount = post.Count();
        
        return new SearchPostDto
        {
            Posts = post.ToList(),
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
            throw new ArgumentException("Search Term cannot be null or empty");
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
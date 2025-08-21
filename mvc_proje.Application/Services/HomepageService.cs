using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.ContactMessage;
using mvc_proje.Application.Dtos.Admin.Homepage;
using mvc_proje.Application.Dtos.Admin.Post;
using mvc_proje.Application.Dtos.ContactMessage;
using mvc_proje.Application.Dtos.Post;
using mvc_proje.Application.Validators.ContactMessage;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services;

public class HomepageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ContactMessageCreateValidator _contactMessageCreateValidator;

    public HomepageService(IUnitOfWork unitOfWork,
        ContactMessageCreateValidator contactMessageCreateValidator = null)
    {
        _unitOfWork = unitOfWork;
        _contactMessageCreateValidator = contactMessageCreateValidator ?? new ContactMessageCreateValidator();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, includeFunc: q =>
            q.Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.StockTransactions));
        if (product == null)
        {
            throw new KeyNotFoundException("Ürün bulunamadı");
        }

        return product;
    }

    public async Task<IEnumerable<LoadMoreProductDto>> LoadMoreProducts(int pageNumber, int categoryId = 0)
    {
        var products = await _unitOfWork.ProductRepository.GetPagedAsync(pageNumber, includeFunc: q =>
            q.Include(p => p.Images), predicate: p => categoryId == 0 || p.CategoryId == categoryId);

        return products.Select(p => new LoadMoreProductDto
        {
            Title = p.Name,
            Slug = p.Slug,
            Price = p.Price,
            MainImageUrl = p.GetMainImage().ImageUrl,
        });
    }

    public async Task<Post> GetPostByIdAsync(int id)
    {
        var post = await _unitOfWork.PostRepository.GetByIdAsync(id, includeFunc: q =>
            q.Include(p => p.Tags));
        if (post == null)
        {
            throw new KeyNotFoundException("Yazı bulunamadı");
        }

        return post;
    }

    public async Task CreateContactMessageAsync(ContactMessageCreateDto contactMessageCreate)
    {
        var validationResult = await _contactMessageCreateValidator.ValidateAsync(contactMessageCreate);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var contactMessage = new ContactMessage
        {
            Name = contactMessageCreate.Name,
            Email = contactMessageCreate.Email,
            Subject = contactMessageCreate.Subject,
            Message = contactMessageCreate.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.ContactMessageRepository.AddAsync(contactMessage);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<HomepageDto> GetAdminHomepageDataAsync()
    {
        var homepageDto = new HomepageDto
        {
            OrderCount = await _unitOfWork.OrderRepository.CountAsync(),
            ProductCount = await _unitOfWork.ProductRepository.CountAsync(),
            UserCount = await _unitOfWork.UserRepository.CountAsync(),
            ReviewCount = await _unitOfWork.ReviewRepository.CountAsync(),
            PostCount = await _unitOfWork.PostRepository.CountAsync(),
            CommentCount = await _unitOfWork.CommentRepository.CountAsync(),
            ContactMessageCount = await _unitOfWork.ContactMessageRepository.CountAsync()
        };

        // todo: optimize these queries by adding take parameter to the repository methods
        homepageDto.RecentOrders =
            (await _unitOfWork.OrderRepository.GetAllAsync(includeFunc: o => o.Include(o => o.OrderItems)))
            .OrderByDescending(o => o.CreatedAt)
            .Take(5);
        homepageDto.RecentUsers =
            (await _unitOfWork.UserRepository.GetAllAsync()).OrderByDescending(u => u.CreatedAt).Take(5);
        homepageDto.RecentComments =
            (await _unitOfWork.CommentRepository.GetAllAsync(includeFunc: c =>
                c.Include(c => c.User)
                    .Include(c => c.Post)
            ))
            .OrderByDescending(c => c.CreatedAt).Take(5);
        homepageDto.RecentContactMessages = (await _unitOfWork.ContactMessageRepository.GetAllAsync())
            .OrderByDescending(cm => cm.CreatedAt).Take(5);

        return homepageDto;
    }
}
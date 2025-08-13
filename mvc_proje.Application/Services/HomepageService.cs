using FluentValidation;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.ContactMessage;
using mvc_proje.Application.Dtos.ContactMessage;
using mvc_proje.Application.Validators.ContactMessage;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;

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
             .Include(p => p.Images));
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found");
        }
        return product;
    }
    
    public async Task<Post> GetPostByIdAsync(int id)
    {
        var post = await _unitOfWork.PostRepository.GetByIdAsync(id, includeFunc: q => 
            q.Include(p => p.Tags));
        if (post == null)
        {
            throw new KeyNotFoundException("Post not found");
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
}
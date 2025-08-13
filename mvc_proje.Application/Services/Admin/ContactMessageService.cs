using mvc_proje.Application.Dtos.Admin.ContactMessage;
using mvc_proje.Application.Dtos.ContactMessage;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.ContactMessage;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services.Admin;

public class ContactMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ContactMessageCreateValidator _contactMessageCreateValidator;

    public ContactMessageService(IUnitOfWork unitOfWork, ContactMessageCreateValidator contactMessageCreateValidator = null)
    {
        _unitOfWork = unitOfWork;
        _contactMessageCreateValidator = contactMessageCreateValidator ?? new ContactMessageCreateValidator();
    }
    
    public async Task<ContactMessageDto> GetAllAsync()
    {
        var messages = await _unitOfWork.ContactMessageRepository.GetAllAsync();
        return new ContactMessageDto
        {
            ContactMessages = messages
        };
    }
    
    public async Task DeleteAsync(int id)
    {
        var message = await _unitOfWork.ContactMessageRepository.GetByIdAsync(id);
        if (message == null)
        {
            throw new KeyNotFoundException($"Contact message with ID {id} not found.");
        }

        await _unitOfWork.ContactMessageRepository.DeleteAsync(id);
    }
    
    public async Task<ContactMessageShowDto> GetByIdAsync(int id)
    {
        var message = await _unitOfWork.ContactMessageRepository.GetByIdAsync(id);
        if (message == null)
        {
            throw new KeyNotFoundException($"Contact message with ID {id} not found.");
        }

        return new ContactMessageShowDto 
        {
            Name = message.Name,
            Email = message.Email,
            Subject = message.Subject,
            Message = message.Message
        };
    }
    
    public async Task CreateAsync(ContactMessageCreateDto dto)
    {
        var validationResponse = _contactMessageCreateValidator.Validate(dto);
        if (!validationResponse.IsValid)
        {
            throw new ArgumentException(string.Join(", ", validationResponse.Errors.Select(e => e.ErrorMessage)));
        }
        
        var message = new Domain.Entities.ContactMessage
        {
            Name = dto.Name,
            Email = dto.Email,
            Subject = dto.Subject,
            Message = dto.Message
        };

        await _unitOfWork.ContactMessageRepository.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();
    }
}
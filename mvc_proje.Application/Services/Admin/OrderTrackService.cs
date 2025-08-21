using mvc_proje.Application.Dtos.Admin.OrderTrack;
using mvc_proje.Application.Repositories;
using mvc_proje.Application.Validators.Admin.OrderTrack;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services.Admin;

public class OrderTrackService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderTrackCreateValidator _orderTrackCreateValidator;
    
    public OrderTrackService(
        IUnitOfWork unitOfWork,
        OrderTrackCreateValidator orderTrackCreateValidator = null)
    {
        _unitOfWork = unitOfWork;
        _orderTrackCreateValidator = orderTrackCreateValidator ?? new OrderTrackCreateValidator();
    }
    
    public async Task CreateAsync(OrderTrackCreateDto dto)
    {
        var validationResult = await _orderTrackCreateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException($"Bazı alanlar geçersiz: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}");
        }

        var orderTrack = new Domain.Entities.OrderTrack
        {
            Status = dto.Status,
            TrackingInfo = dto.TrackingInfo,
            OrderId = dto.OrderId,
            CreatedAt = dto.CreatedAt
        };

        await _unitOfWork.OrderTrackRepository.AddAsync(orderTrack);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var orderTrack = await _unitOfWork.OrderTrackRepository.GetByIdAsync(id);
        if (orderTrack == null)
        {
            throw new KeyNotFoundException($"{id} ID'li sipariş takibi bulunamadı.");
        }

        await _unitOfWork.OrderTrackRepository.DeleteAsync(orderTrack.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}
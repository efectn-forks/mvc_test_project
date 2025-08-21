using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Admin.Order;
using mvc_proje.Application.Repositories;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;

namespace mvc_proje.Application.Services.Admin;

public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto> GetAllAsync()
    {
        var orders = await _unitOfWork.OrderRepository.GetAllAsync(includeFunc: q => q
            .Include(o => o.User)
            .Include(o => o.OrderTracks)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product));
        return new OrderDto
        {
            Orders = orders
        };
    }
    
    public async Task<PagedResult<Order>> GetPagedAsync(int pageNumber)
    {
        var totalCount = await _unitOfWork.OrderRepository.CountAsync();
        var orders = await _unitOfWork.OrderRepository.GetPagedAsync(pageNumber, includeFunc: q => q
            .Include(o => o.User)
            .Include(o => o.OrderTracks)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product));

        return new PagedResult<Order>
        {
            Items = orders,
            TotalCount = totalCount,
        };
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _unitOfWork.OrderRepository.GetByIdAsync(id, includeFunc: q => q
            .Include(o => o.User)
            .Include(o => o.OrderTracks)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product));
    }
    
    public async Task DeleteAsync(int id)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
        if (order == null)
        {
            throw new KeyNotFoundException($"{id} ID'li sipariş bulunamadı.");
        }
        
        await _unitOfWork.OrderRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Dtos.Order;
using mvc_proje.Application.Validators.Admin.OrderTrack;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Enums;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class OrderService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderConfirmationDto> GetOrderConfirmation(ClaimsPrincipal user, int orderId)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        
        if (!int.TryParse(user.FindFirst("UserId")?.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID is not available.");
        }
        
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, includeFunc: q => q
            .Include(o => o.OrderItems)
            .Include(o => o.User));
        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }
        
        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to view this order.");
        }

        Console.Write(userId);

        return new OrderConfirmationDto
        {
            Order = order,
            TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice),
            User = order.User,
            LatestTrackStatus = order.GetLatestTrack().Status
        };
    }
    
    public async Task<OrderTrackDto> GetOrderTrackAsync(ClaimsPrincipal user, int orderId)
    {
        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        
        if (!int.TryParse(user.FindFirst("UserId")?.Value, out var userId))
        {
            throw new UnauthorizedAccessException("User ID is not available.");
        }
        
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId, includeFunc: q => q
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .Include(o => o.OrderTracks));
        
        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }
        
        if (order.UserId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to view this order.");
        }

        return new OrderTrackDto
        {
            Order = order,
            User = await _unitOfWork.UserRepository.GetByIdAsync(userId),
            LatestTrackStatus = order.GetLatestTrack().Status,
            TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice)
        };
    }
}
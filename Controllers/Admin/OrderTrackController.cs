using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

public class OrderTrackController : Controller
{
    private readonly OrderTrackRepository _orderTrackRepository;
    
    public OrderTrackController(OrderTrackRepository orderTrackRepository)
    {
        _orderTrackRepository = orderTrackRepository;
    }
    
    [HttpPost]
    [Route("admin/order-track/create")]
    public async Task<IActionResult> Create(CreateOrderTrackViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return RedirectToAction("Show", "Order", new { id = model.OrderId });
        }

        var orderTrack = new OrderTrack
        {
            OrderId = model.OrderId,
            Status = model.Status,
            CreatedAt = model.CreatedAt,
            TrackingInfo = model.TrackingInfo
        };

        await _orderTrackRepository.CreateOrderTrackAsync(orderTrack);
        
        TempData["SuccessMessage"] = "Sipariş takibi başarıyla oluşturuldu.";
        return RedirectToAction("Show", "Order", new { id = model.OrderId });
    }
    
    [HttpGet]
    [Route("admin/order-track/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var orderTrack = await _orderTrackRepository.GetOrderTrackById(id);
        if (orderTrack == null)
        {
            TempData["ErrorMessage"] = "Sipariş takibi bulunamadı.";
            return RedirectToAction("Index", "Order");
        }

        await _orderTrackRepository.DeleteOrderTrackAsync(id);
        
        TempData["SuccessMessage"] = "Sipariş takibi başarıyla silindi.";
        return RedirectToAction("Show", "Order", new { id = orderTrack.OrderId });
    }
}
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.Order;
using mvc_proje.Application.Services;
using ServicesAdmin = mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

public class OrderController : Controller
{
    private readonly ServicesAdmin.OrderService _orderService;
    
    public OrderController(ServicesAdmin.OrderService orderService) 
    {
        _orderService = orderService;
    }
    
    [HttpGet]
    [Route("/admin/orders")]
    public async Task<IActionResult> Index([FromQuery] int page = 1)
    {
        ViewData["Title"] = "Siparişler";
        var orders = await _orderService.GetPagedAsync(page);
        
        ViewData["CurrentPage"] = page;
        ViewData["TotalItems"] = orders.TotalCount;

        var orderDto = new OrderDto
        {
            Orders = orders.Items
        };
        
        return View("Admin/Order/Index", orderDto);
    }
    
    [HttpGet]
    [Route("/admin/orders/{id}")]
    public async Task<IActionResult> Show(int id)
    {
        ViewData["Title"] = "Sipariş Detayı";
        
        try 
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Sipariş bulunamadı.";
                return RedirectToAction("Index");
            }
            
            return View("Admin/Order/Show", new OrderShowDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                User = order.User,
                OrderItems = order.OrderItems,
                OrderTracks = order.OrderTracks,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                LatestTrack = order.GetLatestTrack()
            });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Sipariş detayları alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    
    [HttpGet]
    [Route("/admin/orders/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            await _orderService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Sipariş silinirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
        
        TempData["SuccessMessage"] = "Sipariş başarıyla silindi.";
        return RedirectToAction("Index");
    }
}
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Admin.OrderTrack;
using mvc_proje.Application.Services.Admin;

namespace mvc_proje.Presentation.Controllers.Admin;

public class OrderTrackController : Controller
{
    private readonly OrderTrackService _orderTrackService;
    
    public OrderTrackController(OrderTrackService orderTrackService)
    {
        _orderTrackService = orderTrackService;
    }
    
    [HttpPost]
    [Route("admin/order-track/create")]
    public async Task<IActionResult> Create(OrderTrackCreateDto model)
    {
        try 
        {
            await _orderTrackService.CreateAsync(model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Sipariş takibi oluşturulurken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Show", "Order", new { id = model.OrderId });
        }
        
        TempData["SuccessMessage"] = "Sipariş takibi başarıyla oluşturuldu.";
        return RedirectToAction("Show", "Order", new { id = model.OrderId });
    }
    
    [HttpGet]
    [Route("admin/order-track/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try 
        {
            await _orderTrackService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Sipariş takibi başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Sipariş takibi silinirken bir hata oluştu: {ex.Message}";
        }
        
        return RedirectToAction("Index", "Order");
    }
}
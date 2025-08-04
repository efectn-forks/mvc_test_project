using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Repositories;
using mvc_proje.Models;

namespace mvc_proje.Controllers.Admin;

public class OrderController : Controller
{
    private readonly OrderRepository _orderRepository;
    
    public OrderController(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    [HttpGet]
    [Route("/admin/orders")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Siparişler";
        var orders = await _orderRepository.GetAllOrdersAsync();
        
        return View("Admin/Order/Index", new OrderIndexViewModel{Orders = orders});
    }
    
    [HttpGet]
    [Route("/admin/orders/{id}")]
    public async Task<IActionResult> Show(int id)
    {
        ViewData["Title"] = "Sipariş Detayı";
        var order = await _orderRepository.GetOrderByIdAsync(id);
        
        if (order == null)
        {
            TempData["ErrorMessage"] = "Sipariş bulunamadı.";
            return RedirectToAction("Index");
        }
        
        return View("Admin/Order/Show", new OrderShowViewModel{Order = order});
    }
    
    [HttpGet]
    [Route("/admin/orders/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
        {
            TempData["ErrorMessage"] = "Sipariş bulunamadı.";
            return RedirectToAction("Index");
        }

        await _orderRepository.DeleteOrderAsync(id);
        
        TempData["SuccessMessage"] = "Sipariş başarıyla silindi.";
        return RedirectToAction("Index");
    }
}
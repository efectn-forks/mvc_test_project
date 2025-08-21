using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Cart;
using mvc_proje.Application.Services;

namespace mvc_proje.Presentation.Controllers;

[Authorize(Policy = "UserPolicy")]
public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly OrderService _orderService;

    public CartController(CartService cartService, OrderService orderService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    [HttpGet]
    [Route("/cart")]
    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetAsync(HttpContext.Session);

        return View(cart);
    }

    [HttpPost]
    [Route("/cart/add")]
    public async Task<IActionResult> Add(CartCreateDto model)
    {
        try
        {
            await _cartService.CreateAsync(HttpContext.Session, model);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün sepete eklenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/cart/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _cartService.DeleteAsync(HttpContext.Session, id);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürün sepetten silinirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("/cart/update")]
    public async Task<IActionResult> Update(List<CartCreateDto> models)
    {
        try
        {
            _cartService.UpdateAsync(HttpContext.Session, models);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ürünler güncellenirken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }
    
    [HttpGet]
    [Route("/cart/checkout/payment")]
    public async Task<IActionResult> CheckoutPayment()
    {
        try
        {
            var order = await _cartService.CheckoutIyizicoAsync(HttpContext.Session, Url, User);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Sipariş oluşturulamadı. Sepetinizde ürün bulunmuyor.";
                return RedirectToAction("Index");
            }

            return View(order);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ödeme işlemi sırasında bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
    
    [HttpPost]
    [HttpGet]
    [AllowAnonymous]
    [Route("/cart/checkout/callback/{id}")]
    public async Task<IActionResult> CheckoutCallback(string id)
    {
        try
        {
            var order = await _cartService.CheckoutCallbackAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Ödeme işlemi tamamlanamadı. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Ödeme işlemi sırasında bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    [Route("/cart/order-confirmation/{orderId}")]
    public async Task<IActionResult> OrderConfirmation(int orderId)
    {
        try
        {
            var userId = User.FindFirstValue("UserId");
            var orderConfirmation = await _orderService.GetOrderConfirmation(User, orderId);

            return View(orderConfirmation);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Sipariş onayı alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    [Route("/cart/order-track/{orderId}")]
    public async Task<IActionResult> OrderTrack(int orderId)
    {
        try
        {
            var orderTrack = await _orderService.GetOrderTrackAsync(User, orderId);
            return View(orderTrack);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Sipariş takibi alınırken bir hata oluştu: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}
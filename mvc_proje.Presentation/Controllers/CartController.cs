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
            TempData["ErrorMessage"] = $"An error occurred while adding the product to the cart: {ex.Message}";
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
            TempData["ErrorMessage"] = $"An error occurred while removing the product from the cart: {ex.Message}";
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
            TempData["ErrorMessage"] = $"An error occurred while updating the cart: {ex.Message}";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/cart/checkout")]
    public async Task<IActionResult> Checkout()
    {
        var id = 0;
        try
        {
            var order = await _cartService.CheckoutAsync(HttpContext.Session, User);
            if (order == null)
            {
                TempData["ErrorMessage"] = "No items in the cart to checkout.";
                return RedirectToAction("Index");
            }

            id = order.Id;
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred during checkout: {ex.Message}";
            return RedirectToAction("Index");
        }

        // Redirect to order confirmation page
        return RedirectToAction("OrderConfirmation", new { orderId = id });
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
            TempData["ErrorMessage"] = $"An error occurred while retrieving the order confirmation: {ex.Message}";
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
            TempData["ErrorMessage"] = $"An error occurred while retrieving the order track: {ex.Message}";
            return RedirectToAction("Index");
        }
    }
}
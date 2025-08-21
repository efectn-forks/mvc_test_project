using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Application.Dtos.Cart;
using mvc_proje.Application.Services;
using mvc_proje.Application.Services.Admin;
using mvc_proje.Domain.Entities;

namespace mvc_proje.Presentation.Controllers;

[Authorize(Policy = "UserPolicy")]
public class WishlistController : Controller
{
    private readonly WishlistService _wishlistService;
    private readonly CartService _cartService;
    private readonly ProductService _productService;

    public WishlistController(WishlistService wishlistService,
        CartService cartService,
        ProductService productService)
    {
        _wishlistService = wishlistService;
        _cartService = cartService;
        _productService = productService;
    }

    [HttpGet]
    [Route("wishlist")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("wishlist/json")]
    public async Task<IActionResult> GetWishlistJson()
    {
        try
        {
            var wishlist = await _wishlistService.GetWishlistAsync(User);
            return Json(new { success = true, data = wishlist });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    [Route("wishlist/add/{productId}")]
    public async Task<IActionResult> AddToWishlist(int productId)
    {
        try
        {
            await _wishlistService.AddToWishlistAsync(User, productId);
            return Ok(new { success = true, message = "Ürün başarıyla wishliste eklendi." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    [Route("wishlist/remove/{productId}")]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
        try
        {
            await _wishlistService.RemoveFromWishlistAsync(User, productId);
            return Ok(new { success = true, message = "Ürün wishlistten başarıyla silindi." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    [Route("wishlist/move-cart/{productId}")]
    public async Task<IActionResult> MoveToCart(int productId)
    {
        try
        {
            var wishlistItem = await _wishlistService.GetWishlistAsync(User);
            if (wishlistItem == null || !wishlistItem.WishlistItems.Any(p => p.ProductId == productId))
            {
                return BadRequest(new { success = false, message = "Ürün wishlistte bulunamadı." });
            }

            var product = await _productService.GetByIdAsync2(productId);
            if (product == null)
            {
                return BadRequest(new { success = false, message = "Ürün bulunamadı." });
            }

            // Check if the product is already in the cart
            var cartItems = await _cartService.GetAsync(HttpContext.Session);
            if (cartItems.Items.Any(p => p.ProductId == productId))
            {
                return BadRequest(new { success = false, message = "Ürün zaten sepette." });
            }

            // check if stock is available
            if (product.Stock() < 1)
            {
                return BadRequest(new { success = false, message = "Ürün tükenmiş." });
            }

            await _cartService.CreateAsync(HttpContext.Session, new CartCreateDto
            {
                ProductId = productId,
                Quantity = 1
            });

            await _wishlistService.RemoveFromWishlistAsync(User, productId);

            return Ok(new { success = true, message = "Ürün başarıyla sepete eklendi." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}
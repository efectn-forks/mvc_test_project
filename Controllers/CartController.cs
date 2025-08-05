using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Misc;
using mvc_proje.Models;

namespace mvc_proje.Controllers;

[Authorize(Policy = "UserPolicy")]
public class CartController : Controller
{
    private readonly ProductRepository _productRepository;
    private readonly OrderRepository _orderRepository;
    private readonly UserRepository _userRepository;
    private const string CartSessionKey = "Cart";

    public CartController(ProductRepository productRepository, OrderRepository orderRepository,
        UserRepository userRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    [Route("/cart")]
    public async Task<IActionResult> Index()
    {
        var cart = await getCart();

        return View(cart);
    }

    [HttpPost]
    [Route("/cart/add")]
    public async Task<IActionResult> Add(CartCreateViewModel model)
    {
        Console.WriteLine($"{model.Quantity} {model.ProductId}");
        if (!ModelState.IsValid)
        {
            // print model state errors to console
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return RedirectToAction("Index");
        }

        var product = await _productRepository.GetProductByIdAsync(model.ProductId);
        if (product == null)
        {
            ModelState.AddModelError("", "Product not found.");
            return RedirectToAction("Index");
        }

        if (model.Quantity > product.Stock)
        {
            ModelState.AddModelError("", $"Insufficient stock for product {product.Name}.");
            return RedirectToAction("Index");
        }

        var cart = await getCart();
        cart.AddToCart(product, model.Quantity);
        setCart(cart);

        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/cart/delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cart = await getCart();
        cart.RemoveFromCart(id);
        setCart(cart);

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("/cart/update")]
    public async Task<IActionResult> Update(List<CartCreateViewModel> models)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        var cart = await getCart();

        foreach (var model in models)
        {
            var product = await _productRepository.GetProductByIdAsync(model.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("", "Product not found.");
                continue;
            }

            if (model.Quantity > product.Stock)
            {
                ModelState.AddModelError("", $"Insufficient stock for product {product.Name}.");
                continue;
            }

            // if already exists, update quantity
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == model.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity = model.Quantity;
            }
            else
            {
                // if not exists, add to cart
                cart.AddToCart(product, model.Quantity);
            }
        }

        // check removed items
        var removedItems = cart.Items.Where(i => !models.Any(m => m.ProductId == i.ProductId)).ToList();
        foreach (var item in removedItems)
        {
            cart.RemoveFromCart(item.ProductId);
        }

        setCart(cart);

        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("/cart/checkout")]
    public async Task<IActionResult> Checkout()
    {
        var cart = await getCart();

        var order = new Order();
        var orderItems = new List<OrderItem>();
        foreach (var item in cart.Items)
        {
            orderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.Product.Price
            });
        }

        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

        order.OrderItems = orderItems;
        order.OrderNumber = Guid.NewGuid().ToString();
        order.UserId = (await _userRepository.GetUserByUsername(username)).Id;
        order.CreatedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        var orderTrack = new OrderTrack
        {
            Status = TrackStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        order.OrderTracks = new List<OrderTrack> { orderTrack };
        await _orderRepository.CreateOrderAsync(order);
        
        // Clear the cart after checkout
        cart.Clear();
        setCart(cart);
        
        // Redirect to order confirmation page
        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }
    
    [HttpGet]
    [Route("/cart/order-confirmation/{orderId}")]
    public async Task<IActionResult> OrderConfirmation(int orderId)
    {
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound();
        }
        
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }
        
        if (order.UserId != user.Id)
        {
            return Forbid();
        }
        
        if (order.OrderItems == null || !order.OrderItems.Any())
        {
            ModelState.AddModelError("", "No items in the order.");
            return RedirectToAction("Index");
        }
        
        // Prepare the view model
        var viewModel = new OrderConfirmationViewModel
        {
            Order = order,
            User = user,
            TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice),
            LatestTrackStatus = order.GetLatestTrack().Status
        };
        
        // decrease stock for each product in the order
        foreach (var item in order.OrderItems)
        {
            var product = await _productRepository.GetProductByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock -= item.Quantity;
                await _productRepository.UpdateProductAsync(product);
            }
        }

        return View(viewModel);
    }
    
    [HttpGet]
    [Route("/cart/order-track/{orderId}")]
    public async Task<IActionResult> OrderTrack(int orderId)
    {
        var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound();
        }
        
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }
        
        if (order.UserId != user.Id)
        {
            return Forbid();
        }
        
        return View(new OrderTrackViewModel
        {
            Order = order,
            User = user,
            LatestTrackStatus = order.GetLatestTrack().Status,
            TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice),
        });
    }

    private async Task<ShoppingCart> getCart()
    {
        var session = HttpContext.Session.GetString(CartSessionKey) ?? "{}";
        var sessionDeserialized =
            await JsonSerializer.DeserializeAsync<ShoppingCart>(new MemoryStream(Encoding.UTF8.GetBytes(session))) ??
            new ShoppingCart();

        return sessionDeserialized;
    }

    private void setCart(ShoppingCart cart)
    {
        var jsonSerialized = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString(CartSessionKey, jsonSerialized);
    }
}
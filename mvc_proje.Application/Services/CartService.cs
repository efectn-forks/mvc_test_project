using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using mvc_proje.Application.Dtos.Cart;
using mvc_proje.Application.Validators.Cart;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Enums;
using mvc_proje.Domain.Interfaces;

namespace mvc_proje.Application.Services;

public class CartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CartCreateValidator _cartCreateValidator;
    private const string CartSessionKey = "Cart";
    
    public CartService(IUnitOfWork unitOfWork, CartCreateValidator cartCreateValidator = null)
    {
        _unitOfWork = unitOfWork;
        _cartCreateValidator = cartCreateValidator ?? new CartCreateValidator();
    }
    
    public async Task CreateAsync(ISession sess, CartCreateDto model)
    {
        var validationResult = _cartCreateValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Some fields are invalid: ", string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId);
        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        if (model.Quantity > product.Stock)
        {
            throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");
        }

        var cart = await _getCart(sess);
        cart.AddToCart(product, model.Quantity);
        _setCart(sess, cart);
    }
    
    public async Task DeleteAsync(ISession sess, int productId)
    {
        var cart = await _getCart(sess);
        cart.RemoveFromCart(productId);
        _setCart(sess, cart);
    }
    
    public async Task<ShoppingCart.ShoppingCart> GetAsync(ISession sess)
    {
        return await _getCart(sess);
    }
    
    public async Task UpdateAsync(ISession sess, IEnumerable<CartCreateDto> dtos)
    {
        // TODO: add proper validation
        /*var validationResult = _cartCreateValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Some fields are invalid: ", string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }*/

        var cart = await _getCart(sess);

        foreach (var dto in dtos)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {dto.ProductId} not found.");
            }

            if (dto.Quantity > product.Stock)
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");
            }

            // if already exists, update quantity
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity = dto.Quantity;
            }
            else
            {
                // if not exists, add to cart
                cart.AddToCart(product, dto.Quantity);
            }
        }

        // check removed items
        var removedItems = cart.Items.Where(i => !dtos.Any(m => m.ProductId == i.ProductId)).ToList();
        foreach (var item in removedItems)
        {
            cart.RemoveFromCart(item.ProductId);
        }

        _setCart(sess, cart);
    }
    
    private async Task<ShoppingCart.ShoppingCart> _getCart(ISession sess)
    {
        var session = sess.GetString(CartSessionKey) ?? "{}";
        var sessionDeserialized =
            await JsonSerializer.DeserializeAsync<ShoppingCart.ShoppingCart>(new MemoryStream(Encoding.UTF8.GetBytes(session))) ??
            new ShoppingCart.ShoppingCart();

        return sessionDeserialized;
    }
    
    public async Task<Order> CheckoutAsync(ISession sess, ClaimsPrincipal user)
    {
        var cart = await _getCart(sess);

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
            
            // Update product stock
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");
            }
            
            if (item.Quantity > product.Stock)
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}.");
            }
            
            product.Stock -= item.Quantity;
            await _unitOfWork.ProductRepository.UpdateAsync(product);
        }
        await _unitOfWork.SaveChangesAsync(); // TODO: test here

        var userIdStr = user.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
        if (!int.TryParse(userIdStr, out var userId))
        {
            throw new InvalidOperationException("User ID is not valid.");
        }

        order.OrderItems = orderItems;
        order.OrderNumber = Guid.NewGuid().ToString();
        order.UserId = userId;
        order.CreatedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;

        var orderTrack = new OrderTrack
        {
            Status = TrackStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        order.OrderTracks = new List<OrderTrack> { orderTrack };
        await _unitOfWork.OrderRepository.AddAsync(order);
        
        // Clear the cart after checkout
        cart.Clear();
        _setCart(sess, cart);
        
        return order;
    }

    private void _setCart(ISession sess, ShoppingCart.ShoppingCart cart)
    {
        var jsonSerialized = JsonSerializer.Serialize(cart);
        sess.SetString(CartSessionKey, jsonSerialized);
    }
}
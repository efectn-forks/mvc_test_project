using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mvc_proje.Application.Dtos.Cart;
using mvc_proje.Application.Validators.Cart;
using mvc_proje.Domain.Entities;
using mvc_proje.Domain.Enums;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Domain.Misc;
using Options = Iyzipay.Options;
using OrderItem = mvc_proje.Domain.Entities.OrderItem;

namespace mvc_proje.Application.Services;

public class CartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly CartCreateValidator _cartCreateValidator;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IOptions<IyizicoConfig> _iyizicoConfig;
    private readonly Options _iyizicoOptions;
    private const string CartSessionKey = "Cart";

    public CartService(IUnitOfWork unitOfWork, CartCreateValidator cartCreateValidator = null, 
        IOptions<IyizicoConfig> iyizicoConfig = null)
    {
        _unitOfWork = unitOfWork;
        _cartCreateValidator = cartCreateValidator ?? new CartCreateValidator();
        _jsonSerializerOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        _iyizicoConfig = iyizicoConfig;
        _iyizicoOptions = new Options();
        _iyizicoOptions.ApiKey = _iyizicoConfig.Value.ApiKey;
        _iyizicoOptions.SecretKey = _iyizicoConfig.Value.SecretKey;
        _iyizicoOptions.BaseUrl = _iyizicoConfig.Value.BaseUrl;
    }

    public async Task CreateAsync(ISession sess, CartCreateDto model)
    {
        var validationResult = _cartCreateValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException("Some fields are invalid: ",
                string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(model.ProductId, includeFunc:
            q => q.Include(p => p.Images));
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
            await JsonSerializer.DeserializeAsync<ShoppingCart.ShoppingCart>(
                new MemoryStream(Encoding.UTF8.GetBytes(session))) ??
            new ShoppingCart.ShoppingCart();

        return sessionDeserialized;
    }

    public async Task<CheckoutFormInitialize> CheckoutIyizicoAsync(ISession sess, IUrlHelper url, ClaimsPrincipal user)
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
        }

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

        var user2 = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        if (user2 == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var totalPrice = orderItems.Sum(i => i.Quantity * i.UnitPrice).ToString("F2").Replace(",", ".");

        CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = order.OrderNumber;
        request.Price = totalPrice;
        request.PaidPrice = totalPrice;
        request.Currency = Currency.TRY.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
        request.CallbackUrl = url.Action("CheckoutCallback", "Cart", new { id = order.OrderNumber }, 
            url.ActionContext.HttpContext.Request.Scheme);
        request.Buyer = new Buyer();
        request.Buyer.Id = user2.Id.ToString();
        request.Buyer.Name = user2.FullName;
        request.Buyer.Surname = user2.FullName;
        request.Buyer.Email = user2.Email;
        request.Buyer.IdentityNumber = user2.IdentifyNumber;
        request.Buyer.RegistrationAddress = user2.Address;
        request.Buyer.City = user2.City;
        request.Buyer.Country = user2.Country;

        request.BillingAddress = new Address
        {
            ContactName = user2.FullName,
            City = user2.City,
            Country = user2.Country,
            Description = user2.Address
        };

        request.ShippingAddress = new Address
        {
            ContactName = user2.FullName,
            City = user2.City,
            Country = user2.Country,
            Description = user2.Address
        };

        var basketItems = new List<BasketItem>();
        foreach (var item in orderItems)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId, includeFunc:
                q => q.Include(p => p.Category));
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");
            }

            basketItems.Add(new BasketItem
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Category1 = product.Category.Name,
                ItemType = BasketItemType.PHYSICAL.ToString(),
                Price = (item.UnitPrice * item.Quantity).ToString("F2").Replace(",", "."),
            });
        }

        request.BasketItems = basketItems;

        var result = CheckoutFormInitialize.Create(request, _iyizicoOptions).Result;
        order.PaymentToken = result.Token;
        order.PaymentStatus = PaymentStatus.Pending;
        
        await _unitOfWork.OrderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<Order> CheckoutCallbackAsync(string conversationId)
    {
        var order = (await _unitOfWork.OrderRepository.FindAsync(o => o.OrderNumber == conversationId, includeFunc:
            q => q.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images))).FirstOrDefault();
        if (order == null)
        {
            throw new KeyNotFoundException("Order not found for the provided conversation id.");
        }
        
        if (order.PaymentStatus == PaymentStatus.Completed)
        {
            return order;
        }
        
        RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = order.OrderNumber;
        request.Token = order.PaymentToken;

        CheckoutForm checkoutForm = await CheckoutForm.Retrieve(request, _iyizicoOptions);
        if (checkoutForm.Status == "success")
        {
            order.PaymentStatus = PaymentStatus.Completed;
            order.UpdatedAt = DateTime.UtcNow;

            // Update order items stock
            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");
                }

                product.Stock -= item.Quantity;
                await _unitOfWork.ProductRepository.UpdateAsync(product);
            }

            // add order track
            var orderTrack = new OrderTrack
            {
                Status = TrackStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            order.OrderTracks = new List<OrderTrack> { orderTrack };

            await _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        order.PaymentStatus = PaymentStatus.Failed;
        order.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.OrderRepository.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        throw new InvalidOperationException($"Payment failed: {checkoutForm.ErrorMessage}");
    }

    private void _setCart(ISession sess, ShoppingCart.ShoppingCart cart)
    {
        var jsonSerialized = JsonSerializer.Serialize(cart, _jsonSerializerOptions);
        sess.SetString(CartSessionKey, jsonSerialized);
    }
}
using Microsoft.Extensions.Caching.Distributed;
using RedisCart.Dtos;
using RedisCart.Extensions;

namespace RedisCart.Services;

public interface ICartService
{
    public Task<ShoppingCartDto> GetCartAsync(string userId);
    public Task<ShoppingCartDto> AddItemAsync(string userId, LineItemDto lineItem);
    public Task<ShoppingCartDto> RemoveItemAsync(string userId, string itemId);
    public Task<ShoppingCartDto> ChangeQuantityAsync(string userId, LineItemDto lineItem);
    public Task<ShoppingCartDto> CheckoutCartAsync(string userId);
}

public class CartService : ICartService
{
    private readonly IDistributedCache _cache;

    public CartService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<ShoppingCartDto> GetCartAsync(string userId)
    {
        var cart = await _cache.GetRecordAsync<ShoppingCartDto>(userId);
        cart ??= await CreateCartAsync(userId);

        return cart;
    }

    public async Task<ShoppingCartDto> AddItemAsync(string userId, LineItemDto lineItem)
    {
        var cart = await _cache.GetRecordAsync<ShoppingCartDto>(userId);
        cart ??= await CreateCartAsync(userId);
        cart.LineItems.Add(lineItem);

        await _cache.SetRecordAsync(userId, cart);

        return cart;
    }

    public async Task<ShoppingCartDto> RemoveItemAsync(string userId, string itemId)
    {
        var cart = await _cache.GetRecordAsync<ShoppingCartDto>(userId);

        if (cart is null)
            return await CreateCartAsync(userId);

        var lineItem = cart.LineItems.Find(li => li.Id == itemId);
        if (lineItem is not null)
            cart.LineItems.Remove(lineItem);

        await _cache.SetRecordAsync(userId, cart);

        return cart;
    }

    public async Task<ShoppingCartDto> ChangeQuantityAsync(string userId, LineItemDto lineItem)
    {
        var cart = await _cache.GetRecordAsync<ShoppingCartDto>(userId);

        if (cart is null)
        {
            cart = await CreateCartAsync(userId);
            cart.LineItems.Add(lineItem);

            await _cache.SetRecordAsync(userId, cart);

            return cart;
        }

        if (cart.LineItems.Find(li => li.Id == lineItem.Id) is LineItemDto cartLineItem)
        {
            cartLineItem.Quantity = lineItem.Quantity;
        }
        else
        {
            Console.WriteLine($"Line item {lineItem.Id} not found in cart of user {userId}. Adding it to the cart with the new quantity...");
            cart.LineItems.Add(lineItem);
        }

        await _cache.SetRecordAsync(userId, cart);

        return cart;
    }

    public async Task<ShoppingCartDto> CheckoutCartAsync(string userId)
    {
        return await CreateCartAsync(userId);
    }

    private async Task<ShoppingCartDto> CreateCartAsync(string userId)
    {
        Console.WriteLine($"--> Creating the cart for user {userId}...");

        var cart = new ShoppingCartDto
        {
            UserId = userId,
            LineItems = new List<LineItemDto>()
        };

        await _cache.SetRecordAsync(userId, cart);

        Console.WriteLine($"--> Created the cart for user {userId}");

        return cart;
    }
}

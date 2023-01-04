using Microsoft.AspNetCore.Mvc;
using RedisCart.Dtos;
using RedisCart.Services;

namespace RedisCart.Controllers;

[ApiController]
[Route("api/[controller]/{userId}")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<ShoppingCartDto>> GetUserCart(string userId)
    {
        Console.WriteLine($"--> Getting user cart for user with id {userId}");

        return await _cartService.GetCartAsync(userId);
    }

    [HttpPost("addItem")]
    public async Task<ActionResult<ShoppingCartDto>> AddItem(string userId, [FromBody] LineItemDto lineItem)
    {
        Console.WriteLine($"--> Adding item with id {lineItem.Id} to the cart of user {userId}");

        var cart = await _cartService.AddItemAsync(userId, lineItem);

        return Ok(cart);
    }

    [HttpPost("removeItem/{itemId}")]
    public async Task<ActionResult<ShoppingCartDto>> RemoveItem(string userId, string itemId)
    {
        Console.WriteLine($"--> Removing item with id {itemId} from the cart of user {userId}");

        var cart = await _cartService.RemoveItemAsync(userId, itemId);

        return Ok(cart);
    }

    [HttpPut("change")]
    public async Task<ActionResult<ShoppingCartDto>> ChangeQuantity(string userId, [FromBody] LineItemDto lineItem)
    {
        Console.WriteLine($"--> Changing quantity of item {lineItem.Id} to {lineItem.Quantity} in the cart of user {userId}");

        var cart = await _cartService.ChangeQuantityAsync(userId, lineItem);

        return Ok(cart);
    }

    [HttpPut("checkout")]
    public async Task<ActionResult<ShoppingCartDto>> CheckoutCart(string userId)
    {
        Console.WriteLine($"--> Checking out the cart of user {userId}");

        var cart = await _cartService.CheckoutCartAsync(userId);

        return Ok(cart);
    }
}
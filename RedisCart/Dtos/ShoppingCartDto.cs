using System.Text.Json.Serialization;

namespace RedisCart.Dtos;

public class ShoppingCartDto
{
    public string UserId { get; set; } = null!;
    public List<LineItemDto> LineItems { get; set; }
        = new List<LineItemDto>();

    public decimal Price { get => LineItems.Sum(li => li.Price * li.Quantity); }
}
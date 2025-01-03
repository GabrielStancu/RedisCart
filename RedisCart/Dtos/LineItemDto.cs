namespace RedisCart.Dtos;

public class LineItemDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
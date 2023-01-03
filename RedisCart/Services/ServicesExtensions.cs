namespace RedisCart.Services;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICartService, CartService>();
    }
}
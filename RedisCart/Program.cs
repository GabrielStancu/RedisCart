using Microsoft.Extensions.DependencyInjection;
using RedisCart.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(opt => {
    opt.Configuration = builder.Configuration.GetConnectionString("RedisCart");
    opt.InstanceName = "ShopAppCart_";
});
builder.Services.AddCors(opt => {
    opt.AddPolicy(name: "AllowOrigins", builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
builder.Services.AddServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowOrigins");
app.UseAuthorization();
app.MapControllers();

app.Run();

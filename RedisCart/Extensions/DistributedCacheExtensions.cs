using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace RedisCart.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromDays(7),
            SlidingExpiration = unusedExpireTime
        };
        var jsonData = JsonSerializer.Serialize(data);

        await cache.SetStringAsync(recordId, jsonData, options);
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);

        return jsonData is null
            ? default!
            : JsonSerializer.Deserialize<T>(jsonData)!;
    }
}
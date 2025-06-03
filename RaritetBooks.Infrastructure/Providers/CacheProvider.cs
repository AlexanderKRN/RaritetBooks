using Microsoft.Extensions.Caching.Distributed;
using RaritetBooks.Application.Providers;
using System.Collections.Concurrent;
using System.Text.Json;

namespace RaritetBooks.Infrastructure.Providers;

public class CacheProvider : ICacheProvider
{
    private static readonly ConcurrentDictionary<string, bool> cacheKeys = new();

    private readonly IDistributedCache _cache;

    public CacheProvider(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        where T : class
    {
        var cachedValue = await _cache.GetStringAsync(key, ct);
        if (cachedValue is null)
            return null;

        var value = JsonSerializer.Deserialize<T>(cachedValue);

        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken ct = default)
    {
        var stringValue = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, stringValue, ct);
        cacheKeys.TryAdd(key, true);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await _cache.RemoveAsync(key, ct);
        cacheKeys.TryRemove(key, out _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken ct = default)
    {
        var task = cacheKeys.Keys
            .Where(k => k.StartsWith(prefixKey.ToLower()))
            .Select(k => RemoveAsync(k, ct));

        await Task.WhenAll(task);
    }

    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        CancellationToken ct = default)
        where T : class
    {
        var cachedValue = await GetAsync<T>(key, ct);
        if (cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();
        await SetAsync(key, cachedValue, ct);

        return cachedValue;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RaritetBooks.Application.Providers;

namespace RaritetBooks.Infrastructure.Interceptors;

public class CacheInvalidationInterceptor : SaveChangesInterceptor
{
    private readonly ICacheProvider _cacheProvider;

    public CacheInvalidationInterceptor(ICacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        await InvalidateCache(eventData, ct);

        return await base.SavingChangesAsync(eventData, result, ct);
    }

    private async Task InvalidateCache(DbContextEventData eventData, CancellationToken ct)
    {
        if (eventData.Context is null)
            return;

        var entries = eventData.Context.ChangeTracker.Entries()
            .Where(e => e.State
                is EntityState.Added
                or EntityState.Deleted
                or EntityState.Modified);

        foreach (var entry in entries)
        {
            var entityName = entry.Entity.GetType().Name;
            await _cacheProvider.RemoveByPrefixAsync(entityName, ct);
        }
    }
}
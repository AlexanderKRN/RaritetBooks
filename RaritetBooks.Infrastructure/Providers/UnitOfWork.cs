using RaritetBooks.Application.Providers;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Providers;

public class UnitOfWork : IUnitOfWork
{
    private readonly RaritetBooksWriteDbContext _dbContext;

    public UnitOfWork(RaritetBooksWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
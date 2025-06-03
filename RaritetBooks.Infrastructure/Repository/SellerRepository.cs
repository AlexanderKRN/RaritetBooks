using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RaritetBooks.Application.Features.Sellers;
using RaritetBooks.Application.Features.Sellers.UpdateProduct;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Repository;

public class SellerRepository : ISellerRepository
{
    private readonly RaritetBooksWriteDbContext _dbContext;

    public SellerRepository(RaritetBooksWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(UserSeller seller, CancellationToken ct)
    {
        await _dbContext.Sellers.AddAsync(seller, ct);
    }

    public async Task<IReadOnlyList<UserSeller>> GetAllWithPhotos(CancellationToken ct)
    {
        var sellers = await _dbContext.Sellers
            .Include(s => s.Photos)
            .ToListAsync(cancellationToken: ct);

        return sellers;
    }

    public async Task<Result<UserSeller, Error>> GetById(Guid id, CancellationToken ct)
    {
        var seller = await _dbContext.Sellers
            .Include(s => s.Products)
            .Include(s => s.Photos)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken: ct);

        if (seller is null)
            return ErrorList.General.NotFound(id);

        return seller;
    }

    public async Task<IReadOnlyList<Product>> GetProducts(CancellationToken ct)
    {
        var products = await _dbContext.Products
            .Include(p => p.Photos)
            .AsNoTracking()
            .ToListAsync(ct);

        return products;
    }

    public async Task UpdateProductById(UpdateProductRequest request, CancellationToken ct)
    {
        await _dbContext.Products
            .Where(p => p.Id == request.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(s => s.Title, s => request.Title)
                .SetProperty(s => s.Author, s => request.Author)
                .SetProperty(s => s.Description, s => request.Description)
                .SetProperty(s => s.Price, s => request.Price),
                cancellationToken: ct);
    }

    public async Task DeleteProductById(Guid id, CancellationToken ct)
    {
        await _dbContext.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(ct);
    }
}
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Dtos;
using RaritetBooks.Domain.Common;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetProductsOfSellerById;

public class GetProductsOfSellerByIdQuery : IQueryHandler<
    Guid,
    GetProductsOfSellerByIdResponse,
    Error>
{
    private readonly RaritetBooksReadDbContext _dbContext;

    public GetProductsOfSellerByIdQuery(
        RaritetBooksReadDbContext dbContext,
        ILogger<GetProductsOfSellerByIdQuery> logger)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<GetProductsOfSellerByIdResponse, Error>> Handle(
        Guid sellerId,
        CancellationToken ct)
    {
        var products = await _dbContext.Products
            .AsNoTracking()
            .Where(p => p.SellerId == sellerId)
            .Select(p => new ProductDto(
                p.Id,
                p.SellerId,
                p.Title,
                p.Author,
                p.Description,
                p.Price,
                null))
            .ToListAsync(ct);
        if (products is null)
            return ErrorList.General.NotFound();

        return new GetProductsOfSellerByIdResponse(products);
    }
}

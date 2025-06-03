using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Dtos;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Infrastructure.Constants;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Queries.Products.GetProducts;

public class GetProductsQuery : IQueryHandler<
    GetProductsRequest, GetProductsResponse, Error>
{
    private readonly RaritetBooksReadDbContext _dbContext;
    private readonly ILogger<GetProductsQuery> _logger;
    private readonly ICacheProvider _cacheProvider;
    private readonly IMinioProvider _minioProvider;

    public GetProductsQuery(
        RaritetBooksReadDbContext dbContext,
        ILogger<GetProductsQuery> logger,
        ICacheProvider cacheProvider,
        IMinioProvider minioProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _cacheProvider = cacheProvider;
        _minioProvider = minioProvider;
    }

    public async Task<Result<GetProductsResponse, Error>> Handle(
        GetProductsRequest query, CancellationToken ct)
    {
        try
        {
            var productRespnose = await _cacheProvider.GetOrSetAsync(
                CacheKeys.Products,
                async () =>
                {
                    var products = await _dbContext.Products
                        .Include(s => s.Photos)
                        .ToListAsync(ct);

                    List<ProductDto> productDtos = [];

                    foreach (var product in products)
                    {
                        var photoPathes = product.Photos.Select(p => p.Path);

                        var photoUrls = await _minioProvider.GetPhotos(
                            Application.Common.Constants.ProductsPhotoBucket, photoPathes, ct);

                        var dto = new ProductDto(
                            product.Id,
                            product.SellerId,
                            product.Title,
                            product.Author,
                            product.Description,
                            product.Price,
                            photoUrls.Value);

                        productDtos.Add(dto);
                    }

                    return new GetProductsResponse(productDtos, products.Count);
                },
                ct) ?? new([], 0);

            var dtosQuery = productRespnose.Products
                .Where(n => string.IsNullOrWhiteSpace(query.Search) ||
                            n.Title.ToLower().Contains(query.Search.ToLower()) ||
                            n.Author.ToLower().Contains(query.Search.ToLower()) ||
                            n.Description.ToLower().Contains(query.Search.ToLower()));

            Func<ProductDto, object> selectorKey = query.SortItem
                .ToLower() switch
            {
                "author" => p => p.Author,
                "title" => p => p.Title,
                _ => p => p.Id
            };

            dtosQuery = query.SortOrder == "desc"
                ? dtosQuery.OrderByDescending(selectorKey)
                : dtosQuery.OrderBy(selectorKey);

            var dtosQueryList = dtosQuery.ToList();

            var dtosByPage = dtosQueryList
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return new GetProductsResponse(dtosByPage, dtosQueryList.Count());
        }
        catch (Exception e)
        {
            _logger.LogError("Error while activity with cache provider: {message}", e.Message);
            return ErrorList.General.Internal(e.Message);
        }
    }
}
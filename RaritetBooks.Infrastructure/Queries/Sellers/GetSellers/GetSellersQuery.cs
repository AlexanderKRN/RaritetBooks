using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Dtos;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Infrastructure.Constants;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetSellers;

public class GetSellersQuery : IQueryHandler<GetSellersResponse, Error>
{
    private readonly RaritetBooksReadDbContext _dbContext;
    private readonly ILogger<GetSellersQuery> _logger;
    private readonly ICacheProvider _cacheProvider;
    private readonly IMinioProvider _minioProvider;

    public GetSellersQuery(
        RaritetBooksReadDbContext dbContext,
        ILogger<GetSellersQuery> logger,
        ICacheProvider cacheProvider,
        IMinioProvider minioProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _cacheProvider = cacheProvider;
        _minioProvider = minioProvider;
    }

    public async Task<Result<GetSellersResponse, Error>> Handle(CancellationToken ct)
    {
        try
        {
            return await _cacheProvider.GetOrSetAsync(
            CacheKeys.Sellers,
            async () =>
            {
                var sellers = await _dbContext.Sellers
                    .Include(s => s.Photos)
                    .ToListAsync(ct);

                List<SellerDto> sellersDtos = [];

                foreach (var seller in sellers)
                {
                    var photoPathes = seller.Photos.Select(p => p.Path);

                    var photoUrls = await _minioProvider.GetPhotos(
                        Application.Common.Constants.SellersPhotoBucket, photoPathes, ct);

                    var dto = new SellerDto(
                        seller.Id,
                        seller.FirstName,
                        seller.LastName,
                        seller.Patronomic,
                        photoUrls.Value,
                        []);

                    sellersDtos.Add(dto);
                }

                return new GetSellersResponse(sellersDtos);
            },

            ct) ?? new([]);
        }
        catch (Exception e)
        {
            _logger.LogError("Error while activity with cache provider: {message}", e.Message);
            return ErrorList.General.Internal(e.Message);
        }
    }
}
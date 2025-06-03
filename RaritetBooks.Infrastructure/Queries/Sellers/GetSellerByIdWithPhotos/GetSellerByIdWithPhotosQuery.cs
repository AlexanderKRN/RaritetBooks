using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Dtos;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetSellerByIdWithPhotos;

public class GetSellerByIdWithPhotosQuery : IQueryHandler<
    GetSellerByIdWithPhotosRequest, GetSellerByIdWithPhotosResponse, Error>
{
    private readonly IMinioProvider _minioProvider;
    private readonly RaritetBooksReadDbContext _dbContext;

    public GetSellerByIdWithPhotosQuery(
        IMinioProvider minioProvider, RaritetBooksReadDbContext dbContext)
    {
        _minioProvider = minioProvider;
        _dbContext = dbContext;
    }

    public async Task<Result<GetSellerByIdWithPhotosResponse, Error>> Handle(
        GetSellerByIdWithPhotosRequest request, CancellationToken ct)
    {
        var seller = await _dbContext.Sellers
            .Include(s => s.Photos)
            .Include(s => s.SocialContacts)
            .FirstOrDefaultAsync(s => s.Id == request.SellerId, cancellationToken: ct);
        if (seller is null)
            return ErrorList.General.NotFound();

        var photoPathes = seller.Photos.Select(p => p.Path);

        var photoUrls = await _minioProvider.GetPhotos(
            Application.Common.Constants.SellersPhotoBucket,
            photoPathes,
            ct);
        if (photoUrls.IsFailure)
            return photoUrls.Error;

        var sellerDto = new SellerDto(
            seller.Id,
            seller.FirstName,
            seller.LastName,
            seller.Patronomic,
            [],
            seller.SocialContacts.Select(c => new SocialContactDto() {
                Link = c.Link, 
                Social = c.Social }).ToList()
        );

        return new GetSellerByIdWithPhotosResponse(sellerDto);
    }
}
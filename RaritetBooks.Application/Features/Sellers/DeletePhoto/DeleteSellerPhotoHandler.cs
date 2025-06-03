using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Sellers.DeletePhoto;

public class DeleteSellerPhotoHandler : ICommandHandler<DeleteSellerPhotoRequest, bool, Error>
{
    private readonly IMinioProvider _minioProvider;
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSellerPhotoHandler(
        IMinioProvider minioProvider,
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork)
    {
        _minioProvider = minioProvider;
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool, Error>> Handle(
        DeleteSellerPhotoRequest request, HttpContext context, CancellationToken ct)
    {
        var seller = await _sellerRepository.GetById(request.SellerId, ct);
        if (seller.IsFailure)
            return seller.Error;

        var isRemoved = await _minioProvider.RemovePhoto(
            Constants.SellersPhotoBucket,
            request.Path,
            ct);
        if (isRemoved.IsFailure)
            return isRemoved.Error;

        var isDeleted = seller.Value.DeletePhoto(request.Path);
        if (isDeleted.IsFailure)
            return isDeleted.Error;

        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }
}
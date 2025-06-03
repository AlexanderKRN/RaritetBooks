using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Application.Features.Sellers.UploadPhoto;

public class UploadSellerPhotoHandler : ICommandHandler<
    UploadSellerPhotoRequest, string, Error>
{
    private readonly IMinioProvider _minioProvider;
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadSellerPhotoHandler(
        IMinioProvider minioProvider,
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork)
    {
        _minioProvider = minioProvider;
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string, Error>> Handle(
        UploadSellerPhotoRequest request, HttpContext context, CancellationToken ct)
    {
        var seller = await _sellerRepository.GetById(request.SellerId, ct);
        if (seller.IsFailure)
            return seller.Error;

        var photoId = Guid.NewGuid();

        var path = photoId + Path.GetExtension(request.File.FileName);

        var photo = PhotoSeller.Create(
            path,
            request.File.ContentType,
            request.File.Length);
        if (photo.IsFailure)
            return photo.Error;

        var isSuccessUpload = seller.Value.AddPhoto(photo.Value);
        if (isSuccessUpload.IsFailure)
            return isSuccessUpload.Error;

        await using var stream = request.File.OpenReadStream();

        var objectName = await _minioProvider.UploadPhoto(
            Constants.SellersPhotoBucket,
            stream,
            path,
            ct);
        if (objectName.IsFailure)
            return objectName.Error;

        await _unitOfWork.SaveChangesAsync(ct);

        return path;
    }
}
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Application.Features.Sellers.PublishProduct;

public class PublishProductHandler : ICommandHandler<PublishProductRequest, Guid, Error>
{
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMinioProvider _minioProvider;

    public PublishProductHandler(
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork,
        IMinioProvider minioProvider)
    {
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
        _minioProvider = minioProvider;
    }

    public async Task<Result<Guid, Error>> Handle(
        PublishProductRequest request, HttpContext context,  CancellationToken ct)
    {
        var seller = await _sellerRepository.GetById(request.SellerId, ct);
        if (seller.IsFailure)
            return seller.Error;

        var photoFiles = GetPhotoFiles(request.Files);
        if (photoFiles.IsFailure)
            return photoFiles.Error;

        var photos = photoFiles.Value.Select(p => p.PhotoProduct);

        var product = Product.Create(
            request.Title,
            request.Author,
            request.Description,
            request.Price,
            photos);
        
        if (product.IsFailure)
            return product.Error;
        
        seller.Value.PublishProduct(product.Value);

        foreach (ProductPhotoFile photoFile in photoFiles.Value)
        {
            await using var stream = photoFile.File.OpenReadStream();

            var task = await _minioProvider.UploadPhoto(
                Constants.ProductsPhotoBucket,
                stream,
                photoFile.PhotoProduct.Path, 
                ct);
            if (task.IsFailure)
                 return task.Error;
        }
        await _unitOfWork.SaveChangesAsync(ct);
        return seller.Value.Id;
    }

    private Result<List<ProductPhotoFile>, Error> GetPhotoFiles(
        IFormFileCollection fileCollection)
    {
        List<ProductPhotoFile> photos = [];
        foreach (var file in fileCollection)
        {
            var contentType = Path.GetExtension(file.FileName);

            var photo = PhotoProduct.Create(contentType, file.Length);
            if (photo.IsFailure)
                return photo.Error;
            
            photos.Add(new(photo.Value, file));
        }

        return photos;
    }
}
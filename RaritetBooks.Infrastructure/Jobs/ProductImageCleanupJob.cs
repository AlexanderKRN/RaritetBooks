using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Features.Sellers;
using RaritetBooks.Application.Providers;
using System.Reactive.Linq;

namespace RaritetBooks.Infrastructure.Jobs;

public class ProductImageCleanupJob: IProductImageCleanupJob
{
    private readonly IMinioProvider _minioProvider;
    private readonly ISellerRepository _sellerRepository;
    private readonly ILogger<ProductImageCleanupJob> _logger;

    public ProductImageCleanupJob(
        IMinioProvider minioProvider,
        ISellerRepository sellerRepository,
        ILogger<ProductImageCleanupJob> logger)
    {
        _minioProvider = minioProvider;
        _sellerRepository = sellerRepository;
        _logger = logger;
    }

    public async Task ProcessAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        _logger.LogInformation("Cleaning unused images of products in MinIO ...");

        List<string> storagePhotoPaths = [];

        var objectList = _minioProvider.GetObjectsList(
            Application.Common.Constants.ProductsPhotoBucket,
            cancellationToken);

        objectList.Subscribe(item => storagePhotoPaths.Add(item.Key));
        await objectList.LastOrDefaultAsync();

        if (storagePhotoPaths.Count == 0)
        {
            _logger.LogInformation("No images of products to delete in MinIO.");
            return;
        }

        var products = await _sellerRepository.GetProducts(cancellationToken);
        
        var photoPaths = products
            .SelectMany(s => s.Photos)
            .Select(p => p.Path)
            .ToList();

        var extraPaths = storagePhotoPaths
            .Except(photoPaths)
            .ToList();

        if (extraPaths.Count == 0)
        {
            _logger.LogInformation("No images of products to delete in MinIO.");
            return;
        }

        var removeResult = await _minioProvider.RemovePhotos(
            Application.Common.Constants.ProductsPhotoBucket,
            extraPaths,
            cancellationToken);

        if (removeResult.IsFailure)
            _logger.LogError("Error of deleting images from MinIO storage.");

        _logger.LogInformation("Product images have been deleted from MinIO storage.");
    }
}
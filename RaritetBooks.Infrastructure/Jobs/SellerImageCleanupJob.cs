using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Features.Sellers;
using RaritetBooks.Application.Providers;
using System.Reactive.Linq;

namespace RaritetBooks.Infrastructure.Jobs;

public class SellerImageCleanupJob: ISellerImageCleanupJob
{
    private readonly IMinioProvider _minioProvider;
    private readonly ISellerRepository _sellerRepository;
    private readonly ILogger<SellerImageCleanupJob> _logger;

    public SellerImageCleanupJob(
        IMinioProvider minioProvider,
        ISellerRepository sellerRepository,
        ILogger<SellerImageCleanupJob> logger)
    {
        _minioProvider = minioProvider;
        _sellerRepository = sellerRepository;
        _logger = logger;
    }

    public async Task ProcessAsync()
    {
        var cancellationToken = new CancellationTokenSource().Token;

        _logger.LogInformation("Cleaning unused images of sellers in MinIO ...");

        List<string> storagePhotoPaths = [];

        var objectList = _minioProvider.GetObjectsList(
            Application.Common.Constants.SellersPhotoBucket,
            cancellationToken);

        objectList.Subscribe(item => storagePhotoPaths.Add(item.Key));
        await objectList.LastOrDefaultAsync();

        if (storagePhotoPaths.Count == 0)
        {
            _logger.LogInformation("No images of sellers to delete in MinIO.");
            return;
        }

        var sellers = await _sellerRepository.GetAllWithPhotos(cancellationToken);

        var photoPaths = sellers
            .SelectMany(s => s.Photos)
            .Select(p => p.Path)
            .ToList();

        var extraPaths = storagePhotoPaths
            .Except(photoPaths)
            .ToList();

        if (extraPaths.Count == 0)
        {
            _logger.LogInformation("No images of sellers to delete in MinIO.");
            return;
        }

        var removeResult = await _minioProvider.RemovePhotos(
            Application.Common.Constants.SellersPhotoBucket,
            extraPaths,
            cancellationToken);

        if (removeResult.IsFailure)
            _logger.LogError("Error of deleting images from MinIO storage.");

        _logger.LogInformation("Seller images have been deleted from MinIO storage.");
    }
}
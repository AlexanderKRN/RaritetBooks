using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Infrastructure.Providers;

public class MinioProvider : IMinioProvider
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<string, Error>> UploadPhoto(
        string bucketName,
        Stream stream,
        string path,
        CancellationToken ct)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
        
            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, ct);
        
            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
        
                await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
            }
        
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithObject(path);
        
            var response = await _minioClient.PutObjectAsync(putObjectArgs, ct);
        
            return response.ObjectName;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while MinIO activity: {message}", e.Message);
            return ErrorList.General.SaveFailure(bucketName);
        }
    }

    public async Task<Result<bool, Error>> RemovePhoto(
        string bucketName,
        string path,
        CancellationToken ct)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, ct);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
            }

            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(path);

            await _minioClient.RemoveObjectAsync(removeObjectArgs, ct);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while MinIO activity: {message}", e.Message);
            return ErrorList.General.DeleteFailure(bucketName);
        }
    }

    public async Task<Result<IReadOnlyList<string>, Error>> GetPhotos(
        string bucketName,
        IEnumerable<string> paths,
        CancellationToken ct)
    {
        try
        {
            List<string> urls = [];

            foreach (var path in paths)
            {
                var presiggnedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(path)
                    .WithExpiry(60 * 60 * 24);

                var url = await _minioClient.PresignedGetObjectAsync(presiggnedGetObjectArgs);
                urls.Add(url);
            }

            return urls;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while MinIO activity: {message}", e.Message);
            return ErrorList.General.NotFound();
        }
    }

    public IObservable<Item> GetObjectsList(
        string bucketName,
        CancellationToken ct)
    {
        var listObjectsArgs = new ListObjectsArgs().WithBucket(bucketName);

        return _minioClient.ListObjectsAsync(listObjectsArgs, ct);
    }

    public async Task<Result<bool, Error>> RemovePhotos(
        string bucketName,
        List<string> paths,
        CancellationToken ct)
    {
        try
        {
            var bucketExistArgs = new BucketExistsArgs()
                .WithBucket(bucketName);

            var bucketExist = await _minioClient.BucketExistsAsync(bucketExistArgs, ct);

            if (bucketExist == false)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);

                await _minioClient.MakeBucketAsync(makeBucketArgs, ct);
            }

            var removeObjectsArgs = new RemoveObjectsArgs()
                .WithBucket(bucketName)
                .WithObjects(paths);

            await _minioClient.RemoveObjectsAsync(removeObjectsArgs, ct);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Error while MinIO activity: {message}", e.Message);
            return ErrorList.General.DeleteFailure(bucketName);
        }
    }
}
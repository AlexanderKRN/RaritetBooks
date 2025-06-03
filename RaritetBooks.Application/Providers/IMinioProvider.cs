using CSharpFunctionalExtensions;
using Minio.DataModel;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Providers;

public interface IMinioProvider
{
    Task<Result<string, Error>> UploadPhoto(string bucketName, Stream stream, string path, CancellationToken ct);
    Task<Result<bool, Error>> RemovePhoto(string bucketName, string path, CancellationToken ct);
    Task<Result<IReadOnlyList<string>, Error>> GetPhotos(string bucketName, IEnumerable<string> paths, CancellationToken ct);
    IObservable<Item> GetObjectsList(string bucketName, CancellationToken ct);
    Task<Result<bool, Error>> RemovePhotos(string bucketName, List<string> paths, CancellationToken ct);
}
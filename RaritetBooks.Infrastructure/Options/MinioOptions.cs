namespace RaritetBooks.Infrastructure.Options;

public class MinioOptions
{
    public const string Minio = nameof(Minio);
    public string EndPoint { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
}
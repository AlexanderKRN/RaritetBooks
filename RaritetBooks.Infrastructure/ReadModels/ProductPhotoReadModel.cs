namespace RaritetBooks.Infrastructure.ReadModels
{
    public class ProductPhotoReadModel
    {
        public Guid Id { get; init; }
        public Guid ProductId { get; init; }
        public string Path { get; init; } = string.Empty;
        public bool IsMain { get; init; }
    }
}
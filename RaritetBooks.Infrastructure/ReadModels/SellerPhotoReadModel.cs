namespace RaritetBooks.Infrastructure.ReadModels;

public class SellerPhotoReadModel
{
    public Guid Id { get; init; }
    public Guid SellerId { get; init; }
    public string Path { get; init; } = string.Empty;
    public bool IsMain { get; init; }
}
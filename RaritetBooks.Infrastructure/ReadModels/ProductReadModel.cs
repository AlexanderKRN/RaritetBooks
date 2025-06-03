namespace RaritetBooks.Infrastructure.ReadModels;

public class ProductReadModel
{
    public Guid Id { get; init; }
    public Guid SellerId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public List<ProductPhotoReadModel> Photos { get; init; } = []; 
}
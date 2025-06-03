namespace RaritetBooks.Application.Dtos;

public record SellerPhotoDto
{
    public Guid Id { get; init; }
    public string Path { get; init; } = string.Empty;
    public bool IsMain { get; init; }
}
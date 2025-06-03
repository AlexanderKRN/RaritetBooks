namespace RaritetBooks.Application.Dtos;

public record ProductDto(
    Guid Id,
    Guid SellerId,
    string Title,
    string Author,
    string Description,
    decimal Price,
    IReadOnlyList<string>? Photos);
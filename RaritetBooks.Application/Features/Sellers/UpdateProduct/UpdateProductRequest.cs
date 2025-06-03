using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.Sellers.UpdateProduct;

public record UpdateProductRequest(
    Guid Id,
    string Title,
    string Author,
    string Description,
    decimal Price) : ICommand;


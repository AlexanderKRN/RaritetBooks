using Microsoft.AspNetCore.Http;
using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.Sellers.PublishProduct;

public record PublishProductRequest(
    Guid SellerId,
    string Title,
    string Author,
    string Description,
    decimal Price,
    IFormFileCollection Files) : ICommand;
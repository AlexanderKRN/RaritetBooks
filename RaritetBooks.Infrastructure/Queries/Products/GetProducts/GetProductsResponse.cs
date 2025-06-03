using RaritetBooks.Application.Dtos;

namespace RaritetBooks.Infrastructure.Queries.Products.GetProducts;

public record GetProductsResponse(IEnumerable<ProductDto> Products, int TotalCount);
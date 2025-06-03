using RaritetBooks.Application.Dtos;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetProductsOfSellerById
{
    public record GetProductsOfSellerByIdResponse(IEnumerable<ProductDto> Products);
}

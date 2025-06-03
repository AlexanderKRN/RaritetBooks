using RaritetBooks.Application.Dtos;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetSellers;

public record GetSellersResponse(IEnumerable<SellerDto> Sellers);
using RaritetBooks.Application.Common;

namespace RaritetBooks.Infrastructure.Queries.Sellers.GetSellerByIdWithPhotos;

public record GetSellerByIdWithPhotosRequest(Guid SellerId) : IQuery;
using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.Sellers.DeletePhoto;

public record DeleteSellerPhotoRequest(
    Guid SellerId,
    string Path) : ICommand;
using Microsoft.AspNetCore.Http;
using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.Sellers.UploadPhoto;

public record UploadSellerPhotoRequest(
    Guid SellerId,
    IFormFile File) : ICommand;
using Microsoft.AspNetCore.Http;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Application.Common;

public record SellerPhotoFile(
    PhotoSeller PhotoSeller,
    IFormFile File);
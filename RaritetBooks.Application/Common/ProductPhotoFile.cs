using Microsoft.AspNetCore.Http;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Application.Common;

public record ProductPhotoFile(
    PhotoProduct PhotoProduct,
    IFormFile File);
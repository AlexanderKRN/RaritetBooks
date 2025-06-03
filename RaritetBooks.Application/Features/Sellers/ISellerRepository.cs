using CSharpFunctionalExtensions;
using RaritetBooks.Application.Features.Sellers.UpdateProduct;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Application.Features.Sellers
{
    public interface ISellerRepository
    {
        Task Add(UserSeller seller, CancellationToken ct);
        Task DeleteProductById(Guid id, CancellationToken ct);
        Task<IReadOnlyList<UserSeller>> GetAllWithPhotos(CancellationToken ct);
        Task<Result<UserSeller, Error>> GetById(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Product>> GetProducts(CancellationToken ct);
        Task UpdateProductById(UpdateProductRequest request, CancellationToken ct);
    }
}
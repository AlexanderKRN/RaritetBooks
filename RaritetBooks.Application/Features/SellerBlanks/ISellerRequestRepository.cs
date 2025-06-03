using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Application.Features.SellerBlanks
{
    public interface ISellerRequestRepository
    {
        Task<Result<SellerRequest, Error>> GetById(Guid id, CancellationToken ct);
        Task Add(SellerRequest request, CancellationToken ct);
    }
}
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Sellers.DeleteProduct;

public class DeleteProductHandler : ICommandHandler<Guid, Guid, Error>
{
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductHandler(
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork)
    {
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid id, HttpContext context, CancellationToken ct)
    {
        await _sellerRepository.DeleteProductById(id, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return id;
    }
}

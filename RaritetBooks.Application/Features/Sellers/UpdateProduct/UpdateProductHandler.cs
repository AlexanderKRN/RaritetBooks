using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Sellers.UpdateProduct;

public class UpdateProductHandler : ICommandHandler<
    UpdateProductRequest, Guid, Error>
{
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductHandler(
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork)
    {
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateProductRequest request, HttpContext context, CancellationToken ct)
    {
        await _sellerRepository.UpdateProductById(request, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return request.Id;
    }
}

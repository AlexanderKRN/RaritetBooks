using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Domain.ValueObjects;

namespace RaritetBooks.Application.Features.SellerBlanks.Apply;

public class ApplySellerRequestHandler : ICommandHandler<ApplySellerBlankRequest, Guid, Error>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISellerRequestRepository _sellerRequestRepository;
    private readonly ILogger<ApplySellerRequestHandler> _logger;

    public ApplySellerRequestHandler(
        IUnitOfWork unitOfWork,
        ISellerRequestRepository sellerRequestRepository,
        ILogger<ApplySellerRequestHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _sellerRequestRepository = sellerRequestRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        ApplySellerBlankRequest blankRequest,
        HttpContext context,
        CancellationToken ct)
    {
        var fullName = FullName.Create(
            blankRequest.FirstName, blankRequest.LastName, blankRequest.Patronomic).Value;

        var phone = MobilePhone.Create(blankRequest.SellerMobilePhone).Value;
        
        var email = Email.Create(blankRequest.Email).Value;
        
        var form = new SellerRequest(
            fullName,
            phone,
            email,
            blankRequest.Description);

        await _sellerRequestRepository.Add(form, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Seller request created {id}", form.Id);

        return form.Id;
    }
}
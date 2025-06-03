using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Features.Sellers;
using RaritetBooks.Application.Features.Users;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.SellerBlanks.Decline;

public class DeclineSellerBlankHandler : ICommandHandler<DeclineSellerBlankRequest, bool, Error>
{
    private readonly ISellerRequestRepository _sellerRequestRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeclineSellerBlankHandler> _logger;
    private readonly IMessageBus _messageBus;

    public DeclineSellerBlankHandler(
        ISellerRequestRepository sellerRequestRepository,
        IUsersRepository usersRepository,
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeclineSellerBlankHandler> logger,
        IMessageBus messageBus)
    {
        _sellerRequestRepository = sellerRequestRepository;
        _usersRepository = usersRepository;
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _messageBus = messageBus;
    }

    public async Task<Result<bool, Error>> Handle(
        DeclineSellerBlankRequest blankRequest, HttpContext context,  CancellationToken ct)
    {
        var formResult = await _sellerRequestRepository.GetById(blankRequest.Id, ct);
        if (formResult.IsFailure)
            return formResult.Error;

        var form = formResult.Value;

        var declineResult = form.Decline();
        if (declineResult.IsFailure)
            return declineResult.Error;

        await _unitOfWork.SaveChangesAsync(ct);
        
        _logger.LogInformation(
            "Decline for new seller request Id: {id}", form.Id);

        var emailNotification = new EmailNotification(
            "Статус заявки: отказ",
            blankRequest.Comment,
            form.Email);

        await _messageBus.PublishAsync(emailNotification, ct);

        return true;
    }
}
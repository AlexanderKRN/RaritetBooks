using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Features.Users.Logout;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Users.Activate;

public class ActivateHandler: ICommandHandler<Guid, bool, Error>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBus _messageBus;
    private readonly ILogger<LogoutHandler> _logger;

    public ActivateHandler(
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork,
        IMessageBus messageBus,
        ILogger<LogoutHandler> logger)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task<Result<bool, Error>> Handle
        (Guid link, HttpContext context, CancellationToken ct)
    {
        var user = await _usersRepository.GetByActivationLink(link.ToString(), ct);
        if (user.IsFailure)
            return user.Error;

        user.Value.SetActivated();

        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Account for user Id: {id} activated", user.Value.Id);

        var activateNotification = new EmailNotification(
            $"Активация аккаунта на {context.Request.Host} завершена успешно",
            $"<div>\n<h2>Аккаунт успешно активирован!</h2>\n</div>",
            user.Value.Email);

        await _messageBus.PublishAsync(activateNotification, ct);

        return true;
    }
}
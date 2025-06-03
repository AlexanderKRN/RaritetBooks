using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Features.Users;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly IMailProvider _mailProvider;

    public NotificationService(
        ILogger<NotificationService> logger,
        IUsersRepository usersRepository,
        IMailProvider mailProvider)
    {
        _logger = logger;
        _usersRepository = usersRepository;
        _mailProvider = mailProvider;
    }
    
    public async Task<Result<bool, Error>> Notify(Notification notification, CancellationToken ct)
    {
        var user = await _usersRepository.GetById(notification.UserId, ct);
        if (user.IsFailure)
            return ErrorList.General.NotFound();

        var emailNotification = new EmailNotification(
            notification.Subject, notification.Message, user.Value.Email);

        await _mailProvider.SendMessage(emailNotification);

        _logger.LogInformation(notification.UserId.ToString());

        return true;
    }
}
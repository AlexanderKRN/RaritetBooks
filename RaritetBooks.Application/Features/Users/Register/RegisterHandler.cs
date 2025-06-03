using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Domain.ValueObjects;

namespace RaritetBooks.Application.Features.Users.Register;

public class RegisterHandler : ICommandHandler<RegisterRequest, bool, Error>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMessageBus _messageBus;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterHandler> _logger;

    public RegisterHandler(
        IUsersRepository usersRepository,
        IMessageBus messageBus,
        IUnitOfWork unitOfWork,
        ILogger<RegisterHandler> logger)
    {
        _usersRepository = usersRepository;
        _messageBus = messageBus;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool, Error>> Handle(
        RegisterRequest request, HttpContext context, CancellationToken ct)
    {
        var user = await _usersRepository.GetByEmail(request.Email, ct);
        if (!user.IsFailure)
            return ErrorList.Users.AlreadyRegistered(request.Email);

        var email = Email.Create(request.Email);
        if (email.IsFailure)
            return email.Error;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var client = User.CreateClient(email.Value, passwordHash);
        if (client.IsFailure)
            return client.Error;

        await _usersRepository.Add(client.Value, ct);

        client.Value.SetActivationLik(Guid.NewGuid().ToString());

        await _unitOfWork.SaveChangesAsync(ct);
        
        string link = $"http://{context.Request.Host}/api/user/activate/{client.Value.ActivationLink}";
        
        var emailConfirmation = new EmailNotification(
            "Активация аккаунта на " + context.Request.Host,
            $"<div>\n<h2>Для активации перейдите по ссылке</h2>\n<a href={link}>{link}</a>\n</div>",
            client.Value.Email);

        await _messageBus.PublishAsync(emailConfirmation, ct);
        
        _logger.LogInformation(
            "Activation email sent to candidate Id: {id}", client.Value.Id);

        return true;
    }
}
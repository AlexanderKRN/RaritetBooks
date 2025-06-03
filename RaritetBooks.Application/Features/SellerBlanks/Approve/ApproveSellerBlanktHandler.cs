using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PasswordGenerator;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Features.Notifications;
using RaritetBooks.Application.Features.Sellers;
using RaritetBooks.Application.Features.Users;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Infrastructure.Kafka;

namespace RaritetBooks.Application.Features.SellerBlanks.Approve;

public class ApproveSellerBlanktHandler : ICommandHandler<ApproveSellerBlankRequest, bool, Error>
{
    private readonly ISellerRequestRepository _sellerRequestRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveSellerBlanktHandler> _logger;
    private readonly IKafkaProducer<Notification> _kafkaProducer;
    private readonly IMessageBus _messageBus;

    public ApproveSellerBlanktHandler(
        ISellerRequestRepository sellerRequestRepository,
        IUsersRepository usersRepository,
        ISellerRepository sellerRepository,
        IUnitOfWork unitOfWork,
        ILogger<ApproveSellerBlanktHandler> logger,
        IKafkaProducer<Notification> kafkaProducer,
        IMessageBus messageBus)
    {
        _sellerRequestRepository = sellerRequestRepository;
        _usersRepository = usersRepository;
        _sellerRepository = sellerRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _kafkaProducer = kafkaProducer;
        _messageBus = messageBus;
    }

    public async Task<Result<bool, Error>> Handle(
        ApproveSellerBlankRequest blankRequest, HttpContext context, CancellationToken ct)
    {
        var formResult = await _sellerRequestRepository.GetById(blankRequest.Id, ct);
        if (formResult.IsFailure)
            return formResult.Error;

        var form = formResult.Value;

        var approveResult = form.Approve();
        if (approveResult.IsFailure)
            return approveResult.Error;

        var pwd = new Password()
            .IncludeLowercase()
            .IncludeUppercase()
            .IncludeSpecial()
            .LengthRequired(8);
        var password = pwd.Next();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = await _usersRepository.GetByEmail(form.Email.Value, ct);
        if (user.IsFailure)
            return user.Error;

        await _usersRepository.ChangeRoleToSeller(form.Email.Value, passwordHash, ct);

        var seller = UserSeller.Create(
            user.Value.Id,
            form.FullName,
            form.Description,
            form.PhoneNumber,
            []);
        if (seller.IsFailure)
            return seller.Error;

        await _sellerRepository.Add(seller.Value, ct);

        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation(
            "New seller has been approved, created Id: {id}", seller.Value.Id);

        var notification = new Notification(
            user.Value.Id,
            "Статус заявки: одобрено",
            $"Вам присвоен статус: продавец. Новый пароль: {password}");

        await _kafkaProducer.Publish(Constants.KafkaNotificationsTopic, notification, ct);
        
        return true;
    }
}
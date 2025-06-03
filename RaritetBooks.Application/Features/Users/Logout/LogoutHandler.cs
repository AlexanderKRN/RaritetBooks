using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Users.Logout;

public class LogoutHandler : ICommandHandler<bool, Error>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LogoutHandler> _logger;

    public LogoutHandler(
        IJwtProvider jwtProvider,
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork,
        ILogger<LogoutHandler> logger)
    {
        _jwtProvider = jwtProvider;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<bool, Error>> Handle(
        HttpContext context,
        CancellationToken ct)
    {
        context.Request.Cookies.TryGetValue("refresh-token", out var refreshToken);
        if (string.IsNullOrEmpty(refreshToken))
            return ErrorList.Users.InvalidCredentials();
        
        var tokenDataResult = _jwtProvider.Validate(refreshToken);
        if (tokenDataResult.IsFailure)
            return tokenDataResult.Error;
        var tokenData = tokenDataResult.Value;

        var claimUserId = tokenData.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (claimUserId == null)
            return ErrorList.Users.InvalidCredentials();

        var userId = Guid.Parse(claimUserId.Value); 
        
        var user = await _usersRepository.GetById(userId, ct);
        if (user.IsFailure)
            return user.Error;

        if (user.Value.RefreshToken != refreshToken
            || user.Value.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return ErrorList.Users.InvalidCredentials();

        user.Value.DeleteRefreshToken();

        await _unitOfWork.SaveChangesAsync(ct);

        context.Response.Cookies.Delete("refresh-token");
        
        _logger.LogInformation(
            "Refresh token deleted for user Id: {id}", user.Value.Id);

        return true;
    }
        
}
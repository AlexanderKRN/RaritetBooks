using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Users.RefreshToken;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenResponse, Error>
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(
        IJwtProvider jwtProvider,
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork,
        ILogger<RefreshTokenHandler> logger)
    {
        _jwtProvider = jwtProvider;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<RefreshTokenResponse, Error>> Handle(
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

        var user = await _usersRepository.GetById(
            Guid.Parse(claimUserId.Value), ct);
        if (user.IsFailure)
            return user.Error;

        if (user.Value.RefreshToken != refreshToken
            || user.Value.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return ErrorList.Users.InvalidCredentials();
        
        var newTokenDto = _jwtProvider.Generate(user.Value).Value;

        _logger.LogInformation(
            "Access token refreshed for user Id: {id}", user.Value.Id);

        return new RefreshTokenResponse(
            newTokenDto.AccessToken, 
            user.Value.Id, 
            user.Value.Role.Name);
    }
}
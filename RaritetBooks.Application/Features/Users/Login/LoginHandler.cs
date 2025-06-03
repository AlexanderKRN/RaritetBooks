using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RaritetBooks.Application.Common;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Users.Login;

public class LoginHandler : ICommandHandler<LoginRequest, LoginResponse, Error>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        ILogger<LoginHandler> logger)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _logger = logger;
    }

    public async Task<Result<LoginResponse, Error>> Handle(
        LoginRequest request,
        HttpContext context,
        CancellationToken ct)
    {
        var user = await _usersRepository.GetByEmail(request.Email, ct);
        if (user.IsFailure)
            return user.Error;
        if (user.Value.IsActivated == false)
            return ErrorList.Users.NotActivated(request.Email);

        var isVerified = BCrypt.Net.BCrypt.Verify(request.Password, user.Value.PasswordHash);
        if (isVerified == false)
            return ErrorList.Users.InvalidCredentials();

        var tokenDto = _jwtProvider.Generate(user.Value).Value;

        user.Value.SaveRefreshToken(
            tokenDto.RefreshToken,
            tokenDto.RefreshTokenExpiryTime);

        await _unitOfWork.SaveChangesAsync(ct);

        context.Response.Cookies.Append("refresh-token", tokenDto.RefreshToken,
            new CookieOptions
            {
                Expires = tokenDto.RefreshTokenExpiryTime,
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

        _logger.LogInformation(
            "New tokens issued for user Id: {id}", user.Value.Id);

        return new LoginResponse(
            tokenDto.AccessToken, 
            user.Value.Id, 
            user.Value.Role.Name);
    }
}
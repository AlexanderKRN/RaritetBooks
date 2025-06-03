using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RaritetBooks.Application.Dtos;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaritetBooks.Infrastructure.Providers;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public Result<TokenDto, Error> Generate(User user)
    {
        var jwtHandler = new JsonWebTokenHandler();

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var permissionClaims = user.Role.Permissions
            .Select(p => new Claim(Constants.AuthConstants.PERMISSION, p));
        
        var accessClaims = permissionClaims.Concat(
        [
            new(Constants.AuthConstants.USER_ID, user.Id.ToString()),
            new(Constants.AuthConstants.ROLE, user.Role.Name)
        ]);

        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(accessClaims),
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessExpiryMinutes)
        };
        
        var accessToken = jwtHandler.CreateToken(accessTokenDescriptor);

        List<Claim> refreshClaims = [new (Constants.AuthConstants.USER_ID, user.Id.ToString())];

        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(refreshClaims),
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshExpiryDays)
        };
        
        var refreshToken = jwtHandler.CreateToken(refreshTokenDescriptor);

        return new TokenDto(
            accessToken,
            refreshToken,
            refreshTokenDescriptor.Expires);
    }
    
    public Result<ClaimsPrincipal, Error> Validate(string jwt)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            ValidateLifetime = true
        };

        var jwtHandler = new JwtSecurityTokenHandler();

        var jwtData = jwtHandler.ValidateToken(
            jwt,
            validationParameters,
            out SecurityToken securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken is null
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            return ErrorList.Users.InvalidCredentials();

        return jwtData;
    }
}
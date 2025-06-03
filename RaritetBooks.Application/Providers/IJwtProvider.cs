using CSharpFunctionalExtensions;
using RaritetBooks.Application.Dtos;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using System.Security.Claims;

namespace RaritetBooks.Application.Providers;

public interface IJwtProvider
{
    Result<ClaimsPrincipal, Error> Validate(string jwt);
    Result<TokenDto, Error> Generate(User user);
}
using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.Users.Login;

public record LoginRequest(string Email, string Password) : ICommand;
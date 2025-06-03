using RaritetBooks.Application.Common;

namespace RaritetBooks.Application.Features.Users.Register;

public record RegisterRequest(string Email, string Password) : ICommand;
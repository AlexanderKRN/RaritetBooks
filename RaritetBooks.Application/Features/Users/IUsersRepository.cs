using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Application.Features.Users
{
    public interface IUsersRepository
    {
        Task Add(User user, CancellationToken ct);
        Task<Result<User, Error>> GetById(Guid id, CancellationToken ct);
        Task<Result<User, Error>> GetByEmail(string email, CancellationToken ct);
        Task<Result<User, Error>> GetByActivationLink(string link, CancellationToken ct);
        Task ChangeRoleToSeller(string email, string passwordHash, CancellationToken ct);
    }
}
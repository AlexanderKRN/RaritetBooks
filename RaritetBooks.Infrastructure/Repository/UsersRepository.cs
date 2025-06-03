using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using RaritetBooks.Application.Features.Users;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Domain.ValueObjects;
using RaritetBooks.Infrastructure.DbContexts;

namespace RaritetBooks.Infrastructure.Repository;

public class UsersRepository : IUsersRepository
{
    private readonly RaritetBooksWriteDbContext _dbContext;

    public UsersRepository(RaritetBooksWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user, CancellationToken ct)
    {
        await _dbContext.AddAsync(user, ct);
    }

    public async Task<Result<User, Error>> GetById(Guid id, CancellationToken ct)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: ct);

        if (user is null)
            return ErrorList.General.NotFound();

        return user;
    }

    public async Task<Result<User, Error>> GetByEmail(string email, CancellationToken ct)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken: ct);

        if (user is null)
            return ErrorList.General.NotFound();

        return user;
    }

    public async Task<Result<User, Error>> GetByActivationLink(string link, CancellationToken ct)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.ActivationLink == link, cancellationToken: ct);

        if (user is null)
            return ErrorList.General.NotFound();

        return user;
    }

    public async Task ChangeRoleToSeller(
        string email,
        string passwordHash,
        CancellationToken ct)
    {
        var user = await _dbContext.Users
            .Where(u => u.Email.Value == email)
            .ExecuteUpdateAsync(s => s
                .SetProperty(s => s.Role.Name, s => Role.Seller.Name)
                .SetProperty(s => s.Role.Permissions, s => Role.Seller.Permissions)
                .SetProperty(s => s.PasswordHash, s => passwordHash),
            cancellationToken: ct);
    }
}
using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.ValueObjects;
using Entity = RaritetBooks.Domain.Common.Entity;

namespace RaritetBooks.Domain.Entities;

public class User : Entity
{
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }
    public string? ActivationLink { get; private set; }
    public bool IsActivated { get; private set; } = false;
    public DateTime? ActivatedDate { get; private set; }
    public IReadOnlyList<Order> Orders => _orders;
    private readonly List<Order> _orders = [];
    
    private User()
    {
    }

    private User(Email email, string passwordHash, Role role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public static Result<User, Error> CreateSeller(Email email, string passwordHash)
    {
        return new User(email, passwordHash, Role.Seller);
    }
    
    public static Result<User, Error> CreateAdmin(Email email, string passwordHash)
    {
        return new User(email, passwordHash, Role.Admin);
    }
    
    public static Result<User, Error> CreateClient(Email email, string passwordHash)
    {
        return new User(email, passwordHash, Role.Client);
    }

    public void SetActivationLik(string link)
    {
        ActivationLink = link;
    }

    public void SetActivated()
    {
        IsActivated = true;
        ActivatedDate = DateTime.UtcNow;
    }
    
    public void SaveRefreshToken(string refreshToken, DateTime? refreshTokenExpiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
    
    public void DeleteRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }
}
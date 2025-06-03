using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.ValueObjects;
using Entity = RaritetBooks.Domain.Common.Entity;

namespace RaritetBooks.Domain.Entities;

public class UserSeller : Entity
{
    public const int PHOTO_COUNT_LIMIT = 5;

    public FullName FullName { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public MobilePhone PhoneNumber { get; }
    public DateTime? RegisteredDate { get; private set; }
    public int? Rating { get; private set; }
    public IReadOnlyList<PhotoSeller> Photos => _photos;
    private readonly List<PhotoSeller> _photos = [];

    public IReadOnlyList<SocialContact> SocialContacts => _socialContacts;
    private readonly List<SocialContact> _socialContacts = [];

    public IReadOnlyList<Product> Products => _products;
    private readonly List<Product> _products = [];

    private UserSeller()
    {
    }

    private UserSeller(
        Guid userId,
        FullName fullName,
        string description,
        MobilePhone phoneNumber,
        DateTime registrationDate,
        int? rating,
        IEnumerable<SocialContact> contacts) : base(userId)
    {
        FullName = fullName;
        Description = description;
        PhoneNumber = phoneNumber;
        RegisteredDate = registrationDate;
        Rating = rating;
        _socialContacts = contacts.ToList();
    }

    public void PublishProduct(Product product)
    {
        _products.Add(product);
    }

    public Result<bool, Error> AddPhoto(PhotoSeller photoAdd)
    {
        if (_photos.Count >= PHOTO_COUNT_LIMIT)
            return ErrorList.Sellers.PhotoCountLimit();

        _photos.Add(photoAdd);
        return true;
    }

    public Result<bool, Error> DeletePhoto(string path)
    {
        var photo = _photos.FirstOrDefault(p => p.Path.Contains(path));
        if (photo is null)
            return ErrorList.General.NotFound();

        _photos.Remove(photo);
        return true;
    }

    public static Result<UserSeller, Error> Create(
        Guid userId,
        FullName fullName,
        string description,
        MobilePhone phoneNumber,
        IEnumerable<SocialContact> contacts)
    {
        if (userId == Guid.Empty)
            return ErrorList.General.ValueIsInvalid(nameof(userId));

        if (description.IsEmpty() || description.Length > Constraints.LONG_TITLE_LENGTH)
            return ErrorList.General.InvalidLength(nameof(description));

        return new UserSeller(
            userId,
            fullName,
            description,
            phoneNumber,
            DateTime.UtcNow,
            null,
            contacts);
    }
}
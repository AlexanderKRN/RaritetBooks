using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using Entity = RaritetBooks.Domain.Common.Entity;

namespace RaritetBooks.Domain.Entities;

public class Product : Entity
{
    public const int PHOTO_COUNT_MAX = 10;
    public const int PHOTO_COUNT_MIN = 1;

    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public IReadOnlyList<PhotoProduct> Photos => _photos;
    private readonly List<PhotoProduct> _photos = [];

    private Product()
    {
    }

    private Product(
        string title, string author,
        string description, decimal price,
        List<PhotoProduct> photos)
    {
        Title = title;
        Author = author;
        Description = description;
        Price = price;
        _photos = photos;
    }

    public static Result<Product, Error> Create(
        string title,
        string author,
        string description,
        decimal price,
        IEnumerable<PhotoProduct> photos)
    {
        if (title.IsEmpty() || title.Length > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength();

        if (author.IsEmpty() || author.Length > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength();

        if (description.IsEmpty() || description.Length > Constraints.LONG_TITLE_LENGTH)
            return ErrorList.General.InvalidLength();
        
        if (price == 0)
            return ErrorList.General.ValueIsInvalid();

        var photosList = photos.ToList();
        if (photosList.Count is > PHOTO_COUNT_MAX or < PHOTO_COUNT_MIN)
            return ErrorList.Products.PhotoCountLimit();

        return new Product(title, author, description, price, photosList);
    }
}
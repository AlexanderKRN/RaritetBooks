using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using ValueObject = RaritetBooks.Domain.Common.ValueObject;

namespace RaritetBooks.Domain.ValueObjects;

public class FullName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    public string? Patronomic { get; }

    private FullName(string firstName, string lastName, string? patronomic)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronomic = patronomic;
    }

    public static Result<FullName, Error> Create(
        string firstName,
        string lastName,
        string? patronomic)
    {
        firstName = firstName.Trim();
        if (firstName.Length is < 1 or > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength(nameof(FirstName));

        lastName = lastName.Trim();
        if (lastName.Length is < 1 or > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength(nameof(LastName));

        patronomic = patronomic?.Trim();
        if (patronomic?.Length > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength(nameof(Patronomic));

        return new FullName(firstName, lastName, patronomic);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
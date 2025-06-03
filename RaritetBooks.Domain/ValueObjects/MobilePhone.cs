using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using System.Text.RegularExpressions;
using ValueObject = RaritetBooks.Domain.Common.ValueObject;

namespace RaritetBooks.Domain.ValueObjects;

public class MobilePhone : ValueObject
{
    public string Number { get; }

    private MobilePhone(string number)
    {
        Number = number;
    }

    public static Result<MobilePhone, Error> Create(string input)
    {
        input = input.Trim();
        if (input.Length is < Constraints.MINIMUM_TITLE_LENGTH or > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength(nameof(MobilePhone));

        if (Regex.IsMatch(input, Constraints.RUSSIAN_PHONE_REGEX) == false)
            return ErrorList.General.ValueIsInvalid(nameof(MobilePhone));

        return new MobilePhone(input);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
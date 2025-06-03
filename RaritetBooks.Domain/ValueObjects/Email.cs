using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using System.Text.RegularExpressions;
using ValueObject = RaritetBooks.Domain.Common.ValueObject;

namespace RaritetBooks.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string input)
    {
        input = input.Trim();
        if (input.Length is < 1 or > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength(nameof(Email));

        if (Regex.IsMatch(input, "^(.+)@(.+)$") == false)
            return ErrorList.General.ValueIsInvalid(nameof(Email));

        return new Email(input);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
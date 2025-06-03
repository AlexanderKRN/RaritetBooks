using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using ValueObject = RaritetBooks.Domain.Common.ValueObject;

namespace RaritetBooks.Domain.ValueObjects;

public class SocialTypes : ValueObject
{
    public static readonly SocialTypes Telegram = new("TELEGRAM");
    public static readonly SocialTypes WhatsApp = new("WHATSAPP");
    
    private static readonly SocialTypes[] _all = [Telegram, WhatsApp];
    
    public string Value { get; }

    private SocialTypes(string value)
    {
        Value = value;
    }

    public static Result<SocialTypes, Error> Create(string input)
    {
        input = input.Trim();
        if (input.Length is < Constraints.MINIMUM_TITLE_LENGTH or > Constraints.SHORT_TITLE_LENGTH)
            return ErrorList.General.InvalidLength();

        var social = input.Trim().ToUpper();
        if (_all.Any(s => s.Value == social) == false)
            return ErrorList.General.ValueIsInvalid(nameof(SocialTypes));

        return new SocialTypes(social);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
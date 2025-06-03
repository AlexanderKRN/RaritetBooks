using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using ValueObject = RaritetBooks.Domain.Common.ValueObject;

namespace RaritetBooks.Domain.ValueObjects;

public class SocialContact : ValueObject
{
    public string Link { get; private set; } = null!;
    public SocialTypes Types { get; private set; } = null!;

    private SocialContact(string link, SocialTypes types)
    {
        Link = link;
        Types = types;
    }

    public static Result<SocialContact, Error> Create(string link, SocialTypes social)
    {
        link = link.Trim();
        if (link.IsEmpty() || link.Length > Constraints.LONG_TITLE_LENGTH)
            return ErrorList.General.InvalidLength();

        return new SocialContact(link, social);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Link;
        yield return Types;
    }
}
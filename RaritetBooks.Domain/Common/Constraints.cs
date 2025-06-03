namespace RaritetBooks.Domain.Common;

public readonly struct Constraints
{
    public const int SHORT_TITLE_LENGTH = 50;
    public const int MEDIUM_TITLE_LENGTH = 150;
    public const int LONG_TITLE_LENGTH = 300;
    public const int MINIMUM_TITLE_LENGTH = 1;
    public const int MAX_PHOTO_SIZE = 1000000;
    
    public const string RUSSIAN_PHONE_REGEX = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{10,10}$";
}
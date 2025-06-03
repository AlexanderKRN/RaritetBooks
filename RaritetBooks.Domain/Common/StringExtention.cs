namespace RaritetBooks.Domain.Common
{
    public static class StringExtention
    {
        public static bool IsEmpty(this string? stringExtended)
        {
            return string.IsNullOrWhiteSpace(stringExtended);
        }
    }
}
namespace RaritetBooks.Application.Dtos
{
    public record ProductPhotoDto(
        Guid Id,
        string Path,
        bool IsMain);
}
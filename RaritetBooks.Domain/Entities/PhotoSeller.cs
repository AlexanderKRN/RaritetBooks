using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Domain.Entities;

public class PhotoSeller : Photo
{
    private PhotoSeller(string path)
        : base(path)
    {
    }
    
    public static Result<PhotoSeller, Error> Create(
        string path,
        string contentType,
        long length)
    {
        if (!contentType.EndsWith(BMP) &&
            !contentType.EndsWith(JPEG) &&
            !contentType.EndsWith(JPG) &&
            !contentType.EndsWith(PNG))
            return ErrorList.Photos.FileTypeInvalid(contentType);
        
        if (length > Constraints.MAX_PHOTO_SIZE)
            return ErrorList.Photos.FileSizeInvalid();

        return new PhotoSeller(path);
    }
}
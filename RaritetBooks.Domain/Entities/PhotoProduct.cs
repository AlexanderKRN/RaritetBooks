using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Domain.Entities;

public class PhotoProduct : Photo
{
    private PhotoProduct(string path)
        : base(path)
    {
    }
        
    public static Result<PhotoProduct, Error> Create(
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

        var path = Guid.NewGuid() + contentType;
        
        return new PhotoProduct(path);
    }
}
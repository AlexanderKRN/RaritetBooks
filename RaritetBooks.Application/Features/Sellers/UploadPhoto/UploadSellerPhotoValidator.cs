using FluentValidation;
using RaritetBooks.Application.Common;
using RaritetBooks.Domain.Common;

namespace RaritetBooks.Application.Features.Sellers.UploadPhoto;

public class UploadSellerPhotoValidator : AbstractValidator<UploadSellerPhotoRequest>
{
    public UploadSellerPhotoValidator()
    {
        var type = string.Empty;
        long length = 0;

        RuleFor(p => p.File).Must(p =>
            {
                type = p.ContentType;
                return CheckTypes(type);
            })
            .WithError(ErrorList.Photos.FileTypeInvalid(type));

        RuleFor(p => p.File).Must((p =>
            {
                length = p.Length;
                return CheckLength(length);
            }))
            .WithError(ErrorList.Photos.FileSizeInvalid());
    }

    private bool CheckLength(long length)
    {
        return length < Constraints.MAX_PHOTO_SIZE;
    }

    private bool CheckTypes(string contentType)
    {
        string[] allowedContentTypes = ["image/jpeg", "image/png", "image/bmp"];

        return allowedContentTypes.Contains(contentType);
    }
}
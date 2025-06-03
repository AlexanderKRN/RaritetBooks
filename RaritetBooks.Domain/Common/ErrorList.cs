using RaritetBooks.Domain.Entities;

namespace RaritetBooks.Domain.Common;

public static class ErrorList
{
    public static class General
    {
        public static Error Internal(string message)
            => new("internal", message);
        
        public static Error Unauthorized()
            => new("unauthorized", "Пожалуйста авторизуйтесь");

        public static Error Unexpected()
            => new("unexpected", "unexpected");

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $" for Id '{id}'";
            return new("record.not.found", $"Запись не существует{forId}");
        }

        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "Value";
            return new("value.is.invalid", $"{label} is invalid");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name ?? "Value";
            return new("value.is.required", $"{label} is required");
        }

        public static Error InvalidLength(string? name = null)
        {
            var label = name == null ? "" : name + " ";
            return new("invalid.string.length", $"{label}invalid length");
        }

        public static Error SaveFailure(string? name = null)
        {
            var label = name ?? "Value";
            return new("record.save.failure", $"{label} failed to save");
        }
        
        public static Error DeleteFailure(string? name = null)
        {
            var label = name ?? "Value";
            return new("record.delete.failure", $"{label} failed to delete");
        }
    }

    public static class Kafka
    {
        public static Error PersistFail()
        {
            return new("kafka.persist.fail", "Fail to persist message");
        }
    }

    public static class Email
    {
        public static Error FailedToSendEmail()
        {
            return new("email.send.fail", "Fail to send email");
        }
    }

    public static class Photos
    {
        public static Error FileTypeInvalid(string? fileType)
        {
            return new("invalid.file.type", $"This {fileType}: file type is invalid");
        }

        public static Error FileSizeInvalid()
        {
            return new("invalid.file.size", $"Превышен размер файла в 10 мБ");
        }
    }

    public static class Products
    {
        public static Error PhotoCountLimit()
        {
            return new(
                "product.photo.limit",
                $"Limits of photos quantity is {Product.PHOTO_COUNT_MIN} - {Product.PHOTO_COUNT_MAX}");
        }
    }

    public static class Sellers
    {
        public static Error PhotoCountLimit()
        {
            return new("seller.photo.limit", $"Photo count limit is {UserSeller.PHOTO_COUNT_LIMIT}");
        }
    }

    public static class SellerRequest
    {
        public static Error AlreadyApproved() =>
            new("seller.request.already.approved", "Seller request already approved");

        public static Error AlreadyDeclined() =>
            new("seller.request.already.declined", "Seller request already declined");
    }

    public static class Users
    {
        public static Error AlreadyRegistered(string? email)
        {
            return new("user.already.registered", $"{email} уже зарегистрирован");
        }
        
        public static Error NotActivated(string? email)
        {
            return new("account.not.activated", $"Аккаунт не активирован по почте");
        }
        
        public static Error InvalidCredentials()
        {
            return new("invalid.credentials", "Неверные реквизиты");
        }
    }
}
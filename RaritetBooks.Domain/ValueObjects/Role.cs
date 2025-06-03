using RaritetBooks.Domain.Common;

namespace RaritetBooks.Domain.ValueObjects
{
    public class Role : ValueObject
    {
        public static readonly Role Admin = new(
            "ADMIN",
            [
                Common.Permissions.SellerRequests.READ,
                Common.Permissions.SellerRequests.UPDATE,
                Common.Permissions.SellerRequests.DELETE,

                Common.Permissions.Sellers.CREATE,
                Common.Permissions.Sellers.READ,
                Common.Permissions.Sellers.DELETE,

                Common.Permissions.Products.READ,
                Common.Permissions.Products.DELETE
            ]);

        public static readonly Role Seller = new(
            "SELLER",
            [
                Common.Permissions.Sellers.READ,
                Common.Permissions.Sellers.UPDATE,

                Common.Permissions.Products.CREATE,
                Common.Permissions.Products.READ,
                Common.Permissions.Products.UPDATE,
                Common.Permissions.Products.DELETE,
            ]);

        public static readonly Role Client = new(
            "CLIENT",
            [
                Common.Permissions.SellerRequests.CREATE,

                Common.Permissions.Sellers.READ,

                Common.Permissions.Products.READ
            ]);

        public string Name { get; }
        public string[] Permissions { get; }

        private Role(string name, string[] permissions)
        {
            Name = name;
            Permissions = permissions;
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
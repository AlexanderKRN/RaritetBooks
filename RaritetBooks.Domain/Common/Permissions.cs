namespace RaritetBooks.Domain.Common
{
    public class Permissions
    {
        public class Sellers
        {
            public const string CREATE = "sellers.create";
            public const string READ = "sellers.read";
            public const string UPDATE = "sellers.update";
            public const string DELETE = "sellers.delete";
        }
    
        public class Products
        {
            public const string CREATE = "products.create";
            public const string READ = "products.read";
            public const string UPDATE = "products.update";
            public const string DELETE = "products.delete";
        }
    
        public class SellerRequests
        {
            public const string CREATE = "sellerRequests.create";
            public const string READ = "sellerRequests.read";
            public const string UPDATE = "sellerRequests.update";
            public const string DELETE = "sellerRequests.delete";
        }
    }
}
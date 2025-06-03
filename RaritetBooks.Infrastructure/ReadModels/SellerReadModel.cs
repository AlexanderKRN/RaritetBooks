namespace RaritetBooks.Infrastructure.ReadModels
{
    public class SellerReadModel
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string? Patronomic { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public DateTimeOffset RegisteredDate { get; init; }
        public int? Rating { get; private set; }
        
        public List<SellerPhotoReadModel> Photos { get; init; } = [];
        public ICollection<SocialContactReadModel> SocialContacts { get; init; } = [];
        public ICollection<ProductReadModel> Products { get; init; } = [];
    }
}
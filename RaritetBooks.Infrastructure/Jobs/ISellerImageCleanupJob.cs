namespace RaritetBooks.Infrastructure.Jobs;

public interface ISellerImageCleanupJob
{
    Task ProcessAsync();
}
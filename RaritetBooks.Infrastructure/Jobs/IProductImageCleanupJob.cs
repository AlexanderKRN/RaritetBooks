namespace RaritetBooks.Infrastructure.Jobs;

public interface IProductImageCleanupJob
{
    Task ProcessAsync();
}
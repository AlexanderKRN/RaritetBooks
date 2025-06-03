using Hangfire;

namespace RaritetBooks.Infrastructure.Jobs;

public class HangfireWorker
{
    public static void StartRecurringJobs()
    {
        RecurringJob.AddOrUpdate<ISellerImageCleanupJob>(
            "sellers-image-cleaner",
            job => job.ProcessAsync(),
            Cron.Daily);

        RecurringJob.AddOrUpdate<IProductImageCleanupJob>(
            "products-images-cleaner",
            job => job.ProcessAsync(),
            Cron.Hourly());
    }
}
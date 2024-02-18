using EventSourcing.API.BackgroundServices;

namespace EventSourcing.API.Extensions;

public static class WorkerServicesExtensions
{
    public static void AddWorkerServices(this IServiceCollection services)
    {
        services.AddHostedService<ProductReadModelEventStore>();
        services.AddHostedService<ProductReplayEventStore>();
    }
}
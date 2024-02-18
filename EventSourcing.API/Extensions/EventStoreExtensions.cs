using EventStore.ClientAPI;

namespace EventSourcing.API.Extensions;

public static class EventStoreExtensions
{
    public static void AddEventStore(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = EventStoreConnection.Create(connectionString: configuration.GetConnectionString("EventStore"));
        connection.ConnectAsync().Wait();
        services.AddSingleton<IEventStoreConnection>(connection);

        using var logfactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddConsole();
        });

        var logger = logfactory.CreateLogger("Program");

        connection.Connected += (sender, args) =>
        {
            logger.LogInformation("EventStore connection established.");
        };

        connection.ErrorOccurred += (sender, args) =>
        {
            logger.LogError(args.Exception.Message);
        };
    }
    
} 
using System.Text;
using System.Text.Json;
using EventSourcing.API.BackgroundServices.ProductEventStrategy.Factory;
using EventSourcing.API.EventStores;
using EventSourcing.API.Models;
using EventStore.ClientAPI;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace EventSourcing.API.BackgroundServices;

public class ProductReplayEventStore : BackgroundService
{
    private readonly IEventStoreConnection _eventStoreConnection;
    private readonly ILogger<ProductReplayEventStore> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ProductReplayEventStore(IEventStoreConnection eventStoreConnection, ILogger<ProductReplayEventStore> logger, IServiceProvider serviceProvider)
    {
        _eventStoreConnection = eventStoreConnection;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _eventStoreConnection.ConnectToPersistentSubscriptionAsync(ProductStream.StreamName,
            ProductStream.ReplayGroupName, EventAppeared, autoAck:false);
    }
    
    private async Task EventAppeared(EventStorePersistentSubscriptionBase arg1, ResolvedEvent arg2)
    {
        var type = Type.GetType($"{Encoding.UTF8.GetString(arg2.Event.Metadata)}, EventSourcing.Shared");
        
        _logger.LogInformation($"The message processing: {type.ToString()}");

        var eventData = Encoding.UTF8.GetString(arg2.Event.Data);

        var @event = JsonSerializer.Deserialize(eventData, type);

        var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var productStrategyFactory = new ProductModelEventStrategyFactory(@event);
        var productStrategy = productStrategyFactory.GetStrategy();
        productStrategy.ExecuteProductModel(context);
        
        await context.SaveChangesAsync();
        arg1.Acknowledge(arg2.Event.EventId);
        _logger.LogInformation($"Done: {type.ToString()}");
        
    }
}
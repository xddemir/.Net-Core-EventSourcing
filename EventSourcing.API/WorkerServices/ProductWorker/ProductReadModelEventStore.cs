using System.Text;
using System.Text.Json;
using EventSourcing.API.BackgroundServices.ProductEventStrategy;
using EventSourcing.API.BackgroundServices.ProductEventStrategy.Factory;
using EventSourcing.API.EventStores;
using EventSourcing.API.Models;
using EventSourcing.Shared.Events;
using EventStore.ClientAPI;

namespace EventSourcing.API.BackgroundServices;

public class ProductReadModelEventStore : BackgroundService
{
    private readonly IEventStoreConnection _eventStoreConnection;
    private readonly ILogger<ProductReadModelEventStore> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ProductReadModelEventStore(IEventStoreConnection eventStoreConnection, IServiceProvider serviceProvider, ILogger<ProductReadModelEventStore> logger)
    {
        _eventStoreConnection = eventStoreConnection;
        _serviceProvider = serviceProvider;
        _logger = logger;
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
            ProductStream.GroupName, EventAppeared, autoAck:false);
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
        
        // switch (@event)
        // {
        //     case ProductCreatedEvent productCreatedEvent:
        //         product = new Product() {
        //             Name = productCreatedEvent.Name,
        //             Id = productCreatedEvent.Id,
        //             Price = productCreatedEvent.Stock,
        //             UserId = productCreatedEvent.UserId
        //         };
        //         context.Products.Add(product);
        //         break;
        //     
        //     case ProductNameChangeEvent productNameChangeEvent:
        //         product = context.Products.Find(productNameChangeEvent.Id);
        //         if (product != null) product.Name = productNameChangeEvent.ChangedName;
        //         break;
        //     
        //     case ProductPriceChangeEvent productPriceChangeEvent:
        //         product = context.Products.Find(productPriceChangeEvent.Id);
        //         if (product != null) product.Price = productPriceChangeEvent.ChangedPrice;
        //         break;
        //     
        //     case ProductDeletedEvent productDeletedEvent:
        //         product = context.Products.Find(productDeletedEvent.Id);
        //         if (product != null) context.Products.Remove(product);
        //         break;
        // }

        await context.SaveChangesAsync();
        arg1.Acknowledge(arg2.Event.EventId);
        _logger.LogInformation($"Done: {type.ToString()}");
        
    }
    
}   
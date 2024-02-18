using EventSourcing.Shared.Events;

namespace EventSourcing.API.BackgroundServices.ProductEventStrategy.Factory;

public class ProductModelEventStrategyFactory
{
    private object @event;
    public ProductModelEventStrategyFactory(object @event)
    {
        this.@event = @event;
    }

    public IProductModelEventStrategy GetStrategy()
    {
        return @event switch
        {
            ProductCreatedEvent _ => new ProductCreatedEventStrategy(@event as ProductCreatedEvent),
            ProductNameChangeEvent _ => new ProductNameChangeEventStrategy(@event as ProductNameChangeEvent),
            ProductPriceChangeEvent _ => new ProductPriceChangeEventStrategy(@event as ProductPriceChangeEvent),
            ProductDeletedEvent _ => new ProductDeletedEventStrategy(@event as ProductDeletedEvent),
            _ => throw new NotSupportedException("Unsupported event type")
        };
    }
}

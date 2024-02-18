using EventSourcing.API.Models;
using EventSourcing.Shared.Events;

namespace EventSourcing.API.BackgroundServices.ProductEventStrategy;

public class ProductCreatedEventStrategy: IProductModelEventStrategy
{
    private ProductCreatedEvent @event;
    public ProductCreatedEventStrategy(ProductCreatedEvent @event)
    {
        this.@event = @event;
    }
    
    public void ExecuteProductModel(AppDbContext dbContext)
    {
        var product = new Product() {
            Name = @event.Name,
            Id = @event.Id,
            Price = @event.Stock,
            UserId = @event.UserId
        };
        dbContext.Products.Add(product);
    }
}
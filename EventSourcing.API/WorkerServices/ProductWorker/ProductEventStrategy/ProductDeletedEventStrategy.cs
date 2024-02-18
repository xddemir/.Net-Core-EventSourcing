using EventSourcing.API.BackgroundServices.ProductEventStrategy;
using EventSourcing.API.Models;
using EventSourcing.Shared.Events;

namespace EventSourcing.API.BackgroundServices;

public class ProductDeletedEventStrategy: IProductModelEventStrategy
{

    private ProductDeletedEvent @event;
    public ProductDeletedEventStrategy(ProductDeletedEvent @event)
    {
        this.@event = @event;
    }
    
    public void ExecuteProductModel(AppDbContext dbContext)
    {
        var product = dbContext.Products.Find(@event.Id);
        if (product != null) dbContext.Products.Remove(product);
    }
}
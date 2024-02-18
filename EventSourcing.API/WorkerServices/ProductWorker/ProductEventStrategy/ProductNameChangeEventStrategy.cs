using EventSourcing.API.Models;
using EventSourcing.Shared.Events;

namespace EventSourcing.API.BackgroundServices.ProductEventStrategy;

public class ProductNameChangeEventStrategy: IProductModelEventStrategy
{
    private ProductNameChangeEvent @event;

    public ProductNameChangeEventStrategy(ProductNameChangeEvent @event)
    {
        this.@event = @event;
    }

    public void ExecuteProductModel(AppDbContext dbContext)
    {
        var product = dbContext.Products.Find(@event.Id);
        if (product != null) product.Name = @event.ChangedName;
    }
}
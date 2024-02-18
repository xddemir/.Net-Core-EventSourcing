using EventSourcing.API.Models;
using EventSourcing.Shared.Events;

namespace EventSourcing.API.BackgroundServices.ProductEventStrategy;

public class ProductPriceChangeEventStrategy: IProductModelEventStrategy
{
    private ProductPriceChangeEvent @event;

    public ProductPriceChangeEventStrategy(ProductPriceChangeEvent @event)
    {
        this.@event = @event;
    }

    public void ExecuteProductModel(AppDbContext dbContext)
    {
        var product = dbContext.Products.Find(@event.Id);
        if (product != null) product.Price = @event.ChangedPrice;
    }
}
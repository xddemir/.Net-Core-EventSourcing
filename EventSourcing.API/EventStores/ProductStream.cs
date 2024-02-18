using EventSourcing.API.DTOs;
using EventSourcing.Shared.Events;
using EventStore.ClientAPI;

namespace EventSourcing.API.EventStores;

public class ProductStream : AbstractStream
{
    public static string StreamName => "Product Stream";
    public static string GroupName => "agroup";
    public static string ReplayGroupName => "replay";
    
    public ProductStream(IEventStoreConnection eventStoreConnection) : base(eventStoreConnection, StreamName)
    {
    }

    public void Created(CreateProductDto createProductDto)
    {
        Events.AddLast(new ProductCreatedEvent()
        {
            Id = Guid.NewGuid(),
            Name = createProductDto.Name,
            Price = createProductDto.Price,
            Stock = createProductDto.Stock,
            UserId = createProductDto.UserId
        });
    }

    public void NameChanged(ChangeProductNameDto changeProductNameDto)
    {
        Events.AddLast(new ProductNameChangeEvent()
        {
            ChangedName = changeProductNameDto.Name,
            Id = changeProductNameDto.Id
        });
    }

    public void PriceChanged(ChangeProductPriceDto changeProductPriceDto)
    {
        Events.AddLast(new ProductPriceChangeEvent()
        {
            Id = changeProductPriceDto.Id,
            ChangedPrice = changeProductPriceDto.Price
        });
    }

    public void Deleted(Guid id)
    {
        Events.AddLast(new ProductDeletedEvent()
        {
            Id = id
        });
    }
}
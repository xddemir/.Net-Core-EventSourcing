namespace EventSourcing.Shared.Events;

public class ProductPriceChangeEvent : IEvent
{
    public Guid Id { get; set; }
    public decimal ChangedPrice { get; set; }
}
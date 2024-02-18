namespace EventSourcing.Shared.Events;

public class ProductNameChangeEvent : IEvent
{
    public Guid Id { get; set; }
    public string ChangedName { get; set; }
}
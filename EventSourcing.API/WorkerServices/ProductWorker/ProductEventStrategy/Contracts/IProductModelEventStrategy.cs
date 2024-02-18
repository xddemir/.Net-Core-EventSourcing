using EventSourcing.API.Models;
using EventSourcing.Shared.Events;

namespace EventSourcing.API.BackgroundServices.ProductEventStrategy;

public interface IProductModelEventStrategy
{
    void ExecuteProductModel(AppDbContext dbContext);
}
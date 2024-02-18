using EventSourcing.API.DTOs;
using EventSourcing.API.Models;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.API.Handlers.Queries;

public class GetProductAllListByUserIdHandler: IRequestHandler<GetProductAllListByUserId, List<ProductDto>>
{
    private readonly AppDbContext _context;

    public GetProductAllListByUserIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> Handle(GetProductAllListByUserId request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        return products.Select(x => new ProductDto()
        {
            UserId = x.UserId,
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            Stock = x.Stock
        }).ToList();
    }
}
using EventSourcing.API.DTOs;
using MediatR;

namespace EventSourcing.API.Commands;

public class ChangeProductPriceCommand : IRequest<Unit>
{
    public ChangeProductPriceDto ChangeProductPriceDto { get; set; }
}
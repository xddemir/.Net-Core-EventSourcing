using EventSourcing.API.Commands;
using EventSourcing.API.DTOs;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllListByUserId(int userId)
        {
            return Ok(await _mediator.Send(new GetProductAllListByUserId()
            {
                UserId = userId
            }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            await _mediator.Send(new CreateProductCommand()
            {
                CreateProductDto = createProductDto
            });

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductName([FromBody] ChangeProductNameDto changeProductNameDto)
        {
            await _mediator.Send(new ChangeProductNameCommand()
            {
                ChangeProductNameDto = new()
                {
                    Id = changeProductNameDto.Id, 
                    Name = changeProductNameDto.Name,
                }
            });

            return NoContent();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateProductPrice([FromBody] ChangeProductPriceDto changeProductPriceDto)
        {
            await _mediator.Send(new ChangeProductPriceCommand()
            {
               ChangeProductPriceDto = new()
               {
                   Id = changeProductPriceDto.Id,
                   Price = changeProductPriceDto.Price
               }
            });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand()
            {
                Id = id
            });

            return NoContent();
        }
    }
}

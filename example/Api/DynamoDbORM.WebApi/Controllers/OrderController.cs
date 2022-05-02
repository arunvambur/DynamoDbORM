using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DynamoDbORM.Application.Commands.Create;
using DynamoDbORM.Application.Queries;
using System.Threading.Tasks;

namespace DynamoDbORM.WebApi.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;


        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("Order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateOrderResponse))]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderCommand createOrderRequest)
        {
         
            var result = await _mediator.Send(createOrderRequest);
            return Ok(result);
        }

        [HttpGet("order")]
        [ProducesResponseType(typeof(OrderQueryResponse), StatusCodes.Status200OK)]
       
        public async Task<IActionResult> GetOrder([FromQuery] string buyerId)
        {
            var result = await _mediator.Send(new OrderQuery() { Name = buyerId });
            return Ok(result);
        }
    }
}

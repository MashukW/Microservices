using Mango.Services.OrderAPI.Database.Entities;
using Mango.Services.OrderAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.OrderAPI.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{orderId}")]
        public async Task<OrderOutgoing> Get(Guid orderId)
        {
            var order = await _orderService.Get(orderId);
            return order;
        }

        [HttpPut("changeStatus")]
        public async Task<ActionResult> ChangeStatus(Guid orderId, bool status)
        {
            await _orderService.ChangePaymentStatus(orderId, status);
            return Ok();
        }
    }
}
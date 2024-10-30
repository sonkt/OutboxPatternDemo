namespace OutboxPattern.Controllers
{
    // Controllers/OrderController.cs
    using Microsoft.AspNetCore.Mvc;
    using OutboxPattern.Models;
    using OutboxPattern.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await _orderService.CreateOrderAsync(order);
            return Ok(order);
        }

        // GET api/order
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET api/order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // GET api/order/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        // GET api/order/customer/{customerName}
        [HttpGet("customer/{customerName}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByCustomerName(string customerName)
        {
            var orders = await _orderService.GetOrdersByCustomerNameAsync(customerName);
            return Ok(orders);
        }

        // GET api/order/date
        [HttpGet("date")]
        public async Task<ActionResult<List<Order>>> GetOrdersCreatedBetween([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersCreatedBetweenAsync(startDate, endDate);
            return Ok(orders);
        }
    }

}
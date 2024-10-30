namespace OutboxPattern.Services
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using OutboxPattern.Data;
    using OutboxPattern.Models;

    public class OrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderProcessingService _orderProcessingService;
        private readonly PaymentService _paymentService;

        public OrderService(ApplicationDbContext context, OrderProcessingService orderProcessingService, PaymentService paymentService)
        {
            _context = context;
            _orderProcessingService = orderProcessingService;
            _paymentService = paymentService;
        }

        public async Task CreateOrderAsync(Order order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            // Thêm Order vào cơ sở dữ liệu
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Thêm thông báo vào bảng Outbox
            var outboxMessage = new OutboxMessage
            {
                EventType = "OrderCreated",
                Payload = JsonConvert.SerializeObject(order),
                IsProcessed = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.OutboxMessages.Add(outboxMessage);
            await _context.SaveChangesAsync();

            // Commit transaction
            await transaction.CommitAsync();

            // Xử lý đơn hàng và thanh toán
            await _orderProcessingService.ProcessOrderAsync(order.Id);
            await _paymentService.ProcessPaymentAsync(order.Id);
        }

        // Lấy tất cả đơn hàng
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        // Lấy đơn hàng theo ID
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        // Lấy đơn hàng theo trạng thái
        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        // Lấy đơn hàng theo tên khách hàng
        public async Task<List<Order>> GetOrdersByCustomerNameAsync(string customerName)
        {
            return await _context.Orders
                .Where(o => o.CustomerName == customerName)
                .ToListAsync();
        }

        // Lấy đơn hàng đã tạo trong khoảng thời gian
        public async Task<List<Order>> GetOrdersCreatedBetweenAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .ToListAsync();
        }
    }
}
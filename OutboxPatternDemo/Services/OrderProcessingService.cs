namespace OutboxPattern.Services
{
    using Newtonsoft.Json;
    // Services/OrderProcessingService.cs
    using OutboxPattern.Data;
    using OutboxPattern.Models;

    public class OrderProcessingService
    {
        private readonly ApplicationDbContext _context;

        public OrderProcessingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ProcessOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return;

            order.Status = OrderStatus.Processing;
            await _context.SaveChangesAsync();

            // Gửi sự kiện OrderProcessed đến Outbox
            var outboxMessage = new OutboxMessage
            {
                EventType = "OrderProcessed",
                Payload = JsonConvert.SerializeObject(order),
                IsProcessed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.OutboxMessages.Add(outboxMessage);
            await _context.SaveChangesAsync();
        }
    }

}
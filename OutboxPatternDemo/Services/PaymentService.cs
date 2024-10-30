namespace OutboxPattern.Services
{
    using Newtonsoft.Json;
    using OutboxPattern.Data;
    using OutboxPattern.Models;

    public class PaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ProcessPaymentAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return;

            // Giả sử thanh toán thành công
            order.PaymentStatus = PaymentStatus.Paid;
            order.Status = OrderStatus.Completed;
            await _context.SaveChangesAsync();

            // Gửi sự kiện PaymentProcessed đến Outbox
            var outboxMessage = new OutboxMessage
            {
                EventType = "PaymentProcessed",
                Payload = JsonConvert.SerializeObject(order),
                IsProcessed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.OutboxMessages.Add(outboxMessage);
            await _context.SaveChangesAsync();
        }
    }
}
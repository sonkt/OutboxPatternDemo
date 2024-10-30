namespace OutboxPattern.Workers
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OutboxPattern.Data;

    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OutboxProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var pendingMessages = await context.OutboxMessages
                    .Where(m => !m.IsProcessed)
                    .ToListAsync(stoppingToken);

                foreach (var message in pendingMessages)
                {
                    switch (message.EventType)
                    {
                        case "OrderCreated":
                            Console.WriteLine($"Order Created: {message.Payload}");
                            break;
                        case "OrderProcessed":
                            Console.WriteLine($"Order Processed: {message.Payload}");
                            break;
                        case "PaymentProcessed":
                            Console.WriteLine($"Payment Processed: {message.Payload}");
                            break;
                    }

                    // Đánh dấu là đã xử lý
                    message.IsProcessed = true;
                }

                await context.SaveChangesAsync(stoppingToken);
                await Task.Delay(5000, stoppingToken);  // Delay giữa các lần kiểm tra
            }
        }
    }


}
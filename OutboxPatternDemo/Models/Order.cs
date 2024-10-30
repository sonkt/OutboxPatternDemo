namespace OutboxPattern.Models
{
    // Models/Order.cs
    public enum OrderStatus
    {
        Created,
        Processing,
        Completed,
        Failed
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }

    public class Order
    {
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

}
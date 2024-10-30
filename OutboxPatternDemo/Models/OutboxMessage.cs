namespace OutboxPattern.Models
{
    public class OutboxMessage
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
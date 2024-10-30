namespace OutboxPattern.Data
{
    // Data/ApplicationDbContext.cs
    using Microsoft.EntityFrameworkCore;
    using OutboxPattern.Models;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
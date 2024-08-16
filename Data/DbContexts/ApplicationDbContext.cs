using BulkInsertAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkInsertAPI.Data.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Message { get; set; }
        public DbSet<MessageMetadata> MessageMetadata { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add configurations here if needed
            modelBuilder.Entity<Message>()
                .HasIndex(e => new { e.Originator, e.Recipient, e.CharacterSet, e.MessagePartCount, e.SentAt })
                .IsUnique();

            modelBuilder.Entity<MessageMetadata>()
                .HasIndex(e => new { e.MessageId, e.Key, e.Value })
                .IsUnique();
        }
    }
}

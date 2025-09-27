using Microsoft.EntityFrameworkCore;
using ChatTool.Database;
using ChatTool.Database.Models;

namespace ChatTool.Database;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats{ get; set; }
    public DbSet<Message> Messages{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Chats)
            .WithMany(c => c.Participants);

        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Participants)
            .WithMany();

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
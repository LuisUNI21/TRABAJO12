using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .IsRequired();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
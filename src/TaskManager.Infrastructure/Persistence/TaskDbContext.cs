using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItems> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItems>(entity =>
            {
                // Configurar Id como int generado por la base de datos (IDENTITY)
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(t => t.Description)
                      .HasMaxLength(500);

                entity.Property(t => t.Status)
                      .IsRequired();

                entity.Property(t => t.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(t => t.UpdatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("GETDATE()");

                // NO marcar como ValueGeneratedOnAdd: así EF enviará el valor booleano en el INSERT.
                entity.Property(t => t.IsActive)
                      .IsRequired()
                      .HasDefaultValue(true);
            });
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets - tabelas do banco
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<PrioritiesEntity> Priorities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações da tabela Tasks
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Status);
            });

            // Configuração da tabela Priorities
            modelBuilder.Entity<PrioritiesEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).IsRequired().HasMaxLength(7);
                entity.Property(e => e.DisplayOrder).IsRequired();

                // Dados iniciais (seed data)
                entity.HasData(
                    new PrioritiesEntity { Id = 1, Name = "Muito Alta", Color = "#DC2626", DisplayOrder = 1 },
                    new PrioritiesEntity { Id = 2, Name = "Alta", Color = "#F59E0B", DisplayOrder = 2 },
                    new PrioritiesEntity { Id = 3, Name = "Média", Color = "#3B82F6", DisplayOrder = 3 },
                    new PrioritiesEntity { Id = 4, Name = "Baixa", Color = "#10B981", DisplayOrder = 4 },
                    new PrioritiesEntity { Id = 5, Name = "Muito Baixa", Color = "#8B5CF6", DisplayOrder = 5 }
                );
            });
        }
    }
}
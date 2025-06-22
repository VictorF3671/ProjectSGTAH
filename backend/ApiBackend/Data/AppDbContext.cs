using ApiBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ApiBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<User> User { get; set; }
       
        protected AppDbContext()
        {
        }


        public DbSet<Models.Project> Project { get; set; } = default!;
        public DbSet<Models.Tasks> Tasks { get; set; } = default!;
        public DbSet<Models.TimeTracker> TimeTracker { get; set; } = default!;

        public DbSet<Models.Collaborator> Collaborator { get; set; } = default!;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;

            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is User
                    || e.Entity is Project
                    || e.Entity is Tasks        // plural, conforme seu model
                    || e.Entity is TimeTracker
                    || e.Entity is Collaborator
                && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Property("CreatedAt").CurrentValue = utcNow;

                entry.Property("UpdatedAt").CurrentValue = utcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1) Task → Project: cascade delete (opcional, convenção já faz para FK non-nullable)
            modelBuilder.Entity<Models.Tasks>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2) TimeTracker → Task: cascade delete
            modelBuilder.Entity<TimeTracker>()
                .HasOne(tt => tt.Task)
                .WithMany(t => t.TimeTrackers)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3) TimeTracker → Collaborator: cascade delete
            modelBuilder.Entity<TimeTracker>()
                .HasOne(tt => tt.Collaborator)
                .WithMany()
                .HasForeignKey(tt => tt.CollaboratorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    }


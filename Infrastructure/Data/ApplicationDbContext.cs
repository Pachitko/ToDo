using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Core.Domain.Interfaces;
using Core.DomainServices.Abstractions;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IApplicationDbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.StateChanged += UpdateTimestamps;
            ChangeTracker.Tracked += UpdateTimestamps;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>(InitializeEntities);
            builder.Entity<ToDoItem>(InitializeEntities);
            builder.Entity<ToDoList>(InitializeEntities);
        }

        private void InitializeEntities(EntityTypeBuilder<AppUser> builder)
        {
            builder.OwnsOne(u => u.UserProfile, builder =>
            {
                builder.Property(userProfile => userProfile.FirstName).HasMaxLength(64);
                builder.Property(userProfile => userProfile.MiddleName).HasMaxLength(64);
                builder.Property(userProfile => userProfile.LastName).HasMaxLength(64);
            });
            builder.Navigation(u => u.UserProfile).IsRequired();

            builder
                .HasMany(u => u.ToDoLists)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void InitializeEntities(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(i => i.Title).HasMaxLength(128).IsRequired();
            builder
                .HasMany(l => l.ToDoItems)
                .WithOne(i => i.ToDoList)
                .HasForeignKey(i => i.ToDoListId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void InitializeEntities(EntityTypeBuilder<ToDoItem> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.UserId);

            builder.Property(i => i.Title).HasMaxLength(128).IsRequired();
            builder.Property(i => i.CreatedAt).HasDefaultValueSql("now()");
            builder.Property(i => i.ModifiedAt).HasDefaultValueSql("now()");

            builder.OwnsOne(u => u.Recurrence);
        }

        private static void UpdateTimestamps(object sender, EntityEntryEventArgs e)
        {
            if (e.Entry.Entity is IHasTimestamps entityWithTimestamps)
            {
                switch (e.Entry.State)
                {
                    case EntityState.Deleted:
                        entityWithTimestamps.Deleted = DateTime.UtcNow;
                        Console.WriteLine($"Stamped for delete: {e.Entry.Entity}");
                        break;
                    case EntityState.Modified:
                        entityWithTimestamps.Modified = DateTime.UtcNow;
                        Console.WriteLine($"Stamped for update: {e.Entry.Entity}");
                        break;
                    case EntityState.Added:
                        entityWithTimestamps.Added = DateTime.UtcNow;
                        Console.WriteLine($"Stamped for insert: {e.Entry.Entity}");
                        break;
                }
            }
        }
    }
}
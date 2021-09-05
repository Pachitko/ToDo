using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Entities;
using System;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IApplicationDbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ToDoList> ToDoLists { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
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
                builder.Property(userProfile => userProfile.FirstName).HasMaxLength(64).IsRequired(false);
                builder.Property(userProfile => userProfile.LastName).HasMaxLength(64).IsRequired();
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
            builder.HasKey(l => l.Id).IsClustered();
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

            builder.Property(i => i.Title).HasMaxLength(128).IsRequired();
            builder.Property(i => i.Description).HasMaxLength(512);
        }
    }
}

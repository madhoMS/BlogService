using BlogService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Infrastructure.Data
{
    public class BlogPostContext : DbContext
    {
        public BlogPostContext(DbContextOptions<BlogPostContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configurations here

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Reply>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Additional configurations can be set here
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(u => u.Id);
                // If you're using ASP.NET Core Identity, you might not need to configure this entity here
                // as IdentityDbContext comes with its own configuration
            });
        }
    }
}

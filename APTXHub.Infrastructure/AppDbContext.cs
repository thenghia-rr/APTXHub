using APTXHub.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        // Define DbSets for your entities here
        // public DbSet<YourEntity> YourEntities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity properties and relationships here
            // User entity configuration
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // Like entity configuration
            modelBuilder.Entity<Like>()
               .HasKey(l => new { l.PostId, l.UserId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment entity configuration
            modelBuilder.Entity<Comment>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(l => l.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Favorite entity configuration
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.PostId, f.UserId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Post)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

       
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RolesDemo.Models;

namespace RolesDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<MyRegisteredUser> MyRegisteredUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductId).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                // Seed the Product table with ten records
                entity.HasData(
                    new Product
                    {
                        ProductId = "1",
                        Description = "Official Soccer Ball",
                        Price = 19.99M
                    },
                    new Product
                    {
                        ProductId = "2",
                        Description = "Soccer Cleats - Men's",
                        Price = 29.99M
                    },
                    new Product
                    {
                        ProductId = "3",
                        Description = "Youth Soccer Jersey",
                        Price = 39.99M
                    },
                    new Product
                    {
                        ProductId = "4",
                        Description = "Soccer Goal Net",
                        Price = 49.99M
                    },
                    new Product
                    {
                        ProductId = "5",
                        Description = "Training Cones Set",
                        Price = 59.99M
                    },
                    new Product
                    {
                        ProductId = "6",
                        Description = "Soccer Goalkeeper Gloves",
                        Price = 69.99M
                    },
                    new Product
                    {
                        ProductId = "7",
                        Description = "Soccer Shin Guards",
                        Price = 79.99M
                    },
                    new Product
                    {
                        ProductId = "8",
                        Description = "Soccer Training Bibs",
                        Price = 89.99M
                    },
                    new Product
                    {
                        ProductId = "9",
                        Description = "Soccer Coaching Clipboard",
                        Price = 99.99M
                    },
                    new Product
                    {
                        ProductId = "10",
                        Description = "Soccer Agility Ladder",
                        Price = 109.99M
                    }
                );
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });
        }
    }
}
using CoffeeMap.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMap.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CoffeeShop> CoffeeShops => Set<CoffeeShop>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Каскадное удаление: удалили кофейню — ушли её продукты и отзывы
            modelBuilder.Entity<Product>()
                .HasOne(p => p.CoffeeShop)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.CoffeeShopId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.CoffeeShop)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.CoffeeShopId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

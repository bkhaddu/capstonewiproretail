using Microsoft.EntityFrameworkCore;
using RetailOptimizationPlatform.Models;

namespace RetailOptimizationPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>()
    .ToTable(tb => tb.HasTrigger("trg_StockUpdate"));

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductName = "Laptop",
                    Category = "Electronics",
                    Price = 55000,
                    StockQuantity = 15,
                    ReorderLevel = 5
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "Wireless Mouse",
                    Category = "Accessories",
                    Price = 799,
                    StockQuantity = 50,
                    ReorderLevel = 10
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "Smart TV",
                    Category = "TV",
                    Price = 10000,
                    StockQuantity = 19,
                    ReorderLevel = 5
                }
            );
        }
    }
}

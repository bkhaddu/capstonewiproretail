using Microsoft.EntityFrameworkCore;
using RetailOptimizationPlatform.Data;
using RetailOptimizationPlatform.Models;
using RetailOptimizationPlatform.Repositories;
using Xunit;

namespace RetailOptimizationPlatform.Tests
{
    public class ProductRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetLowStockProductsAsync_ReturnsProductsBelowOrAtReorderLevel()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            var lowStockProduct = new Product
            {
                ProductId = 1,
                ProductName = "Low Stock TV",
                Category = "Electronics",
                Price = 500.00m,
                StockQuantity = 3,  // Stock is 3, Reorder level is 5 (Low Stock!)
                ReorderLevel = 5
            };

            var adequateStockProduct = new Product
            {
                ProductId = 2,
                ProductName = "Healthy Stock Laptop",
                Category = "Electronics",
                Price = 1200.00m,
                StockQuantity = 15, // Stock is 15, Reorder level is 5 (Adequate!)
                ReorderLevel = 5
            };

            context.Products.AddRange(lowStockProduct, adequateStockProduct);
            await context.SaveChangesAsync();

            var repository = new ProductRepository(context);

            // Act
            var result = await repository.GetLowStockProductsAsync();

            // Assert
            Assert.NotNull(result);
            var returnedProduct = Assert.Single(result);
            Assert.Equal("Low Stock TV", returnedProduct.ProductName);
        }

        [Fact]
        public async Task CRUD_Operations_WorkCorrectly()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var repository = new ProductRepository(context);

            var product = new Product
            {
                ProductName = "Test Product",
                Category = "Test Category",
                Price = 10.00m,
                StockQuantity = 100,
                ReorderLevel = 10
            };

            // Act: Add
            await repository.AddAsync(product);
            var addedProduct = await repository.GetByIdAsync(product.ProductId);

            // Assert: Add
            Assert.NotNull(addedProduct);
            Assert.Equal("Test Product", addedProduct.ProductName);

            // Act: Update
            addedProduct.Price = 15.00m;
            await repository.UpdateAsync(addedProduct);
            var updatedProduct = await repository.GetByIdAsync(product.ProductId);

            // Assert: Update
            Assert.NotNull(updatedProduct);
            Assert.Equal(15.00m, updatedProduct.Price);

            // Act: Delete
            await repository.DeleteAsync(product.ProductId);
            var deletedProduct = await repository.GetByIdAsync(product.ProductId);

            // Assert: Delete
            Assert.Null(deletedProduct);
        }
    }
}

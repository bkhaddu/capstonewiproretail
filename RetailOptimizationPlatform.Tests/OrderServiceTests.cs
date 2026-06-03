using Microsoft.EntityFrameworkCore;
using RetailOptimizationPlatform.Data;
using RetailOptimizationPlatform.Exceptions;
using RetailOptimizationPlatform.Models;
using RetailOptimizationPlatform.Services;
using Xunit;

namespace RetailOptimizationPlatform.Tests
{
    public class OrderServiceTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Use unique in-memory database name per test to ensure isolation
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task PlaceOrderAsync_ValidOrder_DeductsStockAndCalculatesTotal()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            var product = new Product
            {
                ProductId = 1,
                ProductName = "Wireless Mouse",
                Category = "Electronics",
                Price = 25.00m,
                StockQuantity = 10,
                ReorderLevel = 3
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var orderService = new OrderService(context);

            var order = new Order
            {
                CustomerName = "John Doe",
                CustomerEmail = "john.doe@example.com",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = 1,
                        Quantity = 3
                    }
                }
            };

            // Act
            var result = await orderService.PlaceOrderAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Placed", result.Status);
            Assert.Equal(75.00m, result.TotalAmount); // 3 * 25.00
            Assert.Equal(7, product.StockQuantity); // 10 - 3

            // Verify order is saved in database
            var savedOrder = await context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == result.OrderId);
            Assert.NotNull(savedOrder);
            Assert.Single(savedOrder.OrderItems!);
            Assert.Equal(25.00m, savedOrder.OrderItems!.First().UnitPrice);
        }

        [Fact]
        public async Task PlaceOrderAsync_EmptyOrder_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var orderService = new OrderService(context);

            var order = new Order
            {
                CustomerName = "John Doe",
                CustomerEmail = "john.doe@example.com",
                OrderItems = new List<OrderItem>() // Empty list
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrderProcessingException>(() => orderService.PlaceOrderAsync(order));
            Assert.Equal("Order must contain at least one item.", exception.Message);
        }

        [Fact]
        public async Task PlaceOrderAsync_ProductNotFound_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);
            var orderService = new OrderService(context);

            var order = new Order
            {
                CustomerName = "John Doe",
                CustomerEmail = "john.doe@example.com",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = 999, // Non-existent product
                        Quantity = 1
                    }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ProductNotFoundException>(() => orderService.PlaceOrderAsync(order));
            Assert.Equal("Product not found.", exception.Message);
        }

        [Fact]
        public async Task PlaceOrderAsync_InsufficientStock_ThrowsException()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using var context = new ApplicationDbContext(options);

            var product = new Product
            {
                ProductId = 1,
                ProductName = "Gaming Keyboard",
                Category = "Electronics",
                Price = 80.00m,
                StockQuantity = 2,
                ReorderLevel = 1
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var orderService = new OrderService(context);

            var order = new Order
            {
                CustomerName = "John Doe",
                CustomerEmail = "john.doe@example.com",
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = 1,
                        Quantity = 5 // Requesting 5, only 2 available
                    }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InsufficientStockException>(() => orderService.PlaceOrderAsync(order));
            Assert.Contains("Insufficient stock for Gaming Keyboard", exception.Message);
        }
    }
}

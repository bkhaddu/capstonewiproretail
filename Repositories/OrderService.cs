using Microsoft.EntityFrameworkCore;
using RetailOptimizationPlatform.Data;
using RetailOptimizationPlatform.Models;

namespace RetailOptimizationPlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new Exception("Order must contain at least one item.");
            }

            decimal totalAmount = 0;

            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Insufficient stock for {product.ProductName}");
                }

                product.StockQuantity -= item.Quantity;
                item.UnitPrice = product.Price;
                totalAmount += item.Quantity * product.Price;
            }

            order.TotalAmount = totalAmount;
            order.OrderDate = DateTime.Now;
            order.Status = "Placed";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
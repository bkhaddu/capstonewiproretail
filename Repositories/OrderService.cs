using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RetailOptimizationPlatform.Data;
using RetailOptimizationPlatform.Exceptions;
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
                throw new OrderProcessingException("Order must contain at least one item.");
            }

            IDbContextTransaction? transaction = null;

            if (_context.Database.IsRelational())
            {
                transaction = await _context.Database.BeginTransactionAsync();
            }

            try
            {
                decimal totalAmount = 0;

                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                    if (product == null)
                    {
                        throw new ProductNotFoundException();
                    }

                    if (product.StockQuantity < item.Quantity)
                    {
                        throw new InsufficientStockException(product.ProductName);
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

                if (transaction != null)
                {
                    await transaction.CommitAsync();
                }

                return order;
            }
            catch
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }

                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                }
            }
        }
    }
}

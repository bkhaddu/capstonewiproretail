using Microsoft.EntityFrameworkCore;
using RetailOptimizationPlatform.Data;
using RetailOptimizationPlatform.DTOs;
using RetailOptimizationPlatform.Models;
using System.Data;
using System.Data.Common;

namespace RetailOptimizationPlatform.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => p.StockQuantity <= p.ReorderLevel)
                .ToListAsync();
        }

        public async Task<List<ProductSalesSummary>> GetProductSalesSummaryAsync()
        {
            var summaries = new List<ProductSalesSummary>();
            var connection = _context.Database.GetDbConnection();

            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT 
                        p.ProductId, 
                        p.ProductName, 
                        ISNULL(SUM(oi.Quantity), 0) AS TotalQuantitySold, 
                        ISNULL(SUM(oi.Quantity * oi.UnitPrice), 0) AS TotalRevenue
                    FROM Products p
                    LEFT JOIN OrderItems oi ON p.ProductId = oi.ProductId
                    GROUP BY p.ProductId, p.ProductName";

                command.CommandType = CommandType.Text;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        summaries.Add(new ProductSalesSummary
                        {
                            ProductId = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            TotalQuantitySold = reader.GetInt32(2),
                            TotalRevenue = reader.GetDecimal(3)
                        });
                    }
                }
            }

            return summaries;
        }
    }
}
using RetailOptimizationPlatform.DTOs;
using RetailOptimizationPlatform.Models;

namespace RetailOptimizationPlatform.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<List<Product>> GetLowStockProductsAsync();
        Task<List<ProductSalesSummary>> GetProductSalesSummaryAsync();
    }
}
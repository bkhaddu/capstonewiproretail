using RetailOptimizationPlatform.Models;

namespace RetailOptimizationPlatform.Services
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(Order order);
    }
}
using System.ComponentModel.DataAnnotations;

namespace RetailOptimizationPlatform.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Range(1, 100000)]
        public int Quantity { get; set; }

        [Range(0, 100000)]
        public decimal UnitPrice { get; set; }
    }
}

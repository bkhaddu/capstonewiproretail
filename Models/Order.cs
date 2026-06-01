using System.ComponentModel.DataAnnotations;

namespace RetailOptimizationPlatform.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string CustomerEmail { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Placed";

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
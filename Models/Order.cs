using System.ComponentModel.DataAnnotations;

namespace RetailOptimizationPlatform.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Placed";

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}

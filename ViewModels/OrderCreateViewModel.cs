using System.ComponentModel.DataAnnotations;

namespace RetailOptimizationPlatform.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        [Range(1, 100000)]
        public int Quantity { get; set; } = 1;
    }
}

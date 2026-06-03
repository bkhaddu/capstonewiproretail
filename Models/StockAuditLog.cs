using System.ComponentModel.DataAnnotations;

namespace RetailOptimizationPlatform.Models
{
    public class StockAuditLog
    {
        public int StockAuditLogId { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [Range(0, 100000)]
        public int OldStock { get; set; }

        [Range(0, 100000)]
        public int NewStock { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

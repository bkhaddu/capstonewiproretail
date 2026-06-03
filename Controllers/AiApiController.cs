using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using RetailOptimizationPlatform.Repositories;
using System.Threading.Tasks;

namespace RetailOptimizationPlatform.Controllers
{
    [Route("api/ai")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class AiApiController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public AiApiController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Conceptual AI Agent (MCP) endpoint for stock replenishment ticket summarization.
        /// </summary>
        [HttpGet("summarize-reorder/{productId}")]
        public async Task<IActionResult> SummarizeReorderTicket(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return NotFound(new { message = "Product not found to summarize replenishment ticket." });
            }

            bool isBelowReorder = product.StockQuantity <= product.ReorderLevel;
            string riskLevel = product.StockQuantity == 0 ? "CRITICAL" : (isBelowReorder ? "WARNING" : "LOW");
            int recommendedOrderQty = isBelowReorder ? (product.ReorderLevel * 3) - product.StockQuantity : 0;

            var summary = new
            {
                AgentName = "Cognitive Retail replenishment MCP Agent v1.0",
                TicketId = $"TKT-REPL-{product.ProductId}-{System.DateTime.UtcNow:yyyyMMdd}",
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Status = "Analyzed",
                RiskLevel = riskLevel,
                SummaryText = $"[AI Agent Summary] The product '{product.ProductName}' (Category: '{product.Category}') currently has {product.StockQuantity} units in stock, which is {(isBelowReorder ? "BELOW" : "ABOVE")} its defined reorder level of {product.ReorderLevel} units. Current inventory risk is evaluated as {riskLevel}. {(isBelowReorder ? $"An automated replenishment ticket has been generated recommending a restock of {recommendedOrderQty} units to restore optimal operating levels." : "No immediate restock action is required.")}",
                RecommendedAction = isBelowReorder 
                    ? $"Submit purchase order for {recommendedOrderQty} units of '{product.ProductName}' to prevent stockout leakage." 
                    : "Monitor weekly stock levels."
            };

            return Ok(summary);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailOptimizationPlatform.Data;
using RetailOptimizationPlatform.Models;
using RetailOptimizationPlatform.Services;

namespace RetailOptimizationPlatform.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _context.Products.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int productId, int quantity, string customerName, string customerEmail)
        {
            var order = new Order
            {
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = productId,
                        Quantity = quantity
                    }
                }
            };

            try
            {
                await _orderService.PlaceOrderAsync(order);
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Products = await _context.Products.ToListAsync();
                return View();
            }
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> History()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)!
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return View(orders);
        }
    }
}
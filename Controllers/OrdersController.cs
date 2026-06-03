using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailOptimizationPlatform.Data;
using RetailOptimizationPlatform.Exceptions;
using RetailOptimizationPlatform.Models;
using RetailOptimizationPlatform.Services;
using RetailOptimizationPlatform.ViewModels;

namespace RetailOptimizationPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
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
            await PopulateProductsAsync();
            return View(new OrderCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateProductsAsync();
                return View(model);
            }

            var order = new Order
            {
                CustomerName = model.CustomerName,
                CustomerEmail = model.CustomerEmail,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = model.ProductId,
                        Quantity = model.Quantity
                    }
                }
            };

            try
            {
                await _orderService.PlaceOrderAsync(order);
                return RedirectToAction("Success");
            }
            catch (OrderProcessingException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateProductsAsync();
                return View(model);
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

        private async Task PopulateProductsAsync()
        {
            ViewBag.Products = await _context.Products.ToListAsync();
        }
    }
}

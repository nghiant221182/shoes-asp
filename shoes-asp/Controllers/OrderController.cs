using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoes_asp.Models;

namespace shoes_asp.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId.Value)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(c => c.TotalPrice)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId.Value)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                model.CartItems = cartItems;
                model.GrandTotal = cartItems.Sum(c => c.TotalPrice);
                return View(model);
            }

            var totalAmount = cartItems.Sum(c => c.TotalPrice);

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Chờ xác nhận",
                ShippingAddress = model.ShippingAddress,
                ReceiverName = model.ReceiverName,
                ReceiverPhone = model.ReceiverPhone,
                Note = model.Note,
                UserId = userId.Value
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            var orderDetails = new List<OrderDetail>();

            foreach (var item in cartItems)
            {
                orderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product?.Price ?? 0
                });
            }

            _context.OrderDetails.AddRange(orderDetails);
            _context.SaveChanges();

            _context.Carts.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction("Complete", new { id = order.OrderId });
        }

        public IActionResult Complete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id && o.UserId == userId.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId.Value)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id && o.UserId == userId.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
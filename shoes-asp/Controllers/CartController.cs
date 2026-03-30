using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoes_asp.Models;

namespace shoes_asp.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
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

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1, string? selectedSize = null)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(selectedSize))
            {
                TempData["Error"] = "Vui lòng chọn kích cỡ.";
                return RedirectToAction("Details", "Product", new { id = productId });
            }

            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            var existingCart = _context.Carts.FirstOrDefault(c =>
                c.UserId == userId.Value &&
                c.ProductId == productId &&
                c.SelectedSize == selectedSize);

            if (existingCart != null)
            {
                existingCart.Quantity += quantity;
            }
            else
            {
                var cart = new Cart
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = quantity,
                    SelectedSize = selectedSize
                };

                _context.Carts.Add(cart);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Increase(int cartId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = _context.Carts.FirstOrDefault(c => c.CartId == cartId && c.UserId == userId.Value);
            if (cart == null)
            {
                return NotFound();
            }

            cart.Quantity += 1;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int cartId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.CartId == cartId);
            if (cart == null)
            {
                return NotFound();
            }

            if (cart.Quantity > 1)
            {
                cart.Quantity -= 1;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.CartId == cartId);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
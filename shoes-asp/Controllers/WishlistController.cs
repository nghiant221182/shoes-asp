using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoes_asp.Models;

namespace shoes_asp.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
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

            var wishlists = _context.Wishlists
                .Include(w => w.Product)
                .ThenInclude(p => p!.Brand)
                .Include(w => w.Product)
                .ThenInclude(p => p!.Category)
                .Where(w => w.UserId == userId.Value)
                .ToList();

            return View(wishlists);
        }

        public IActionResult Toggle(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wishlistItem = _context.Wishlists
                .FirstOrDefault(w => w.UserId == userId.Value && w.ProductId == productId);

            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
            }
            else
            {
                var wishlist = new Wishlist
                {
                    UserId = userId.Value,
                    ProductId = productId
                };

                _context.Wishlists.Add(wishlist);
            }

            _context.SaveChanges();

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wishlistItem = _context.Wishlists
                .FirstOrDefault(w => w.WishlistId == id && w.UserId == userId.Value);

            if (wishlistItem == null)
            {
                return NotFound();
            }

            _context.Wishlists.Remove(wishlistItem);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoes_asp.Models;

namespace shoes_asp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? keyword, int? categoryId)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p =>
                    p.Name.Contains(keyword) ||
                    p.Brand.Name.Contains(keyword) ||
                    p.Category.Name.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = query
                .OrderBy(p => p.ProductId)
                .ToList();

            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId;

            if (categoryId.HasValue)
            {
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId.Value);
                ViewBag.CategoryName = category?.Name;
            }

            return View(products);
        }


        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
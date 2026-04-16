using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shoes_asp.Models;

namespace shoes_asp.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly AppDbContext _context;

        public AdminProductController(AppDbContext context)
        {
            _context = context;
        }

        private IActionResult? CheckAdmin()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("UserRole");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (role != "Admin")
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return null;
        }

        public IActionResult Index()
        {
            var access = CheckAdmin();
            if (access != null) return access;

            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .OrderByDescending(p => p.ProductId)
                .ToList();

            return View(products);
        }

        public IActionResult Create()
        {
            var access = CheckAdmin();
            if (access != null) return access;

            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            if (ModelState.IsValid)
            {
                _context.Products.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Thêm sản phẩm thành công.";
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(model.CategoryId, model.BrandId);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            LoadDropdowns(product.CategoryId, product.BrandId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            if (ModelState.IsValid)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == model.ProductId);
                if (product == null) return NotFound();

                product.Name = model.Name;
                product.Price = model.Price;
                product.Image = model.Image;
                product.Description = model.Description;
                product.Stock = model.Stock;
                product.Rate = model.Rate;
                product.Size = model.Size;
                product.CategoryId = model.CategoryId;
                product.BrandId = model.BrandId;

                _context.SaveChanges();
                TempData["Success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(model.CategoryId, model.BrandId);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            TempData["Success"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(int? selectedCategory = null, int? selectedBrand = null)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "CategoryId", "Name", selectedCategory);
            ViewBag.BrandId = new SelectList(_context.Brands.ToList(), "BrandId", "Name", selectedBrand);
        }
    }
}
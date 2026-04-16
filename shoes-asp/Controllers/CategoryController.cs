using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoes_asp.Models;

namespace shoes_asp.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public AdminCategoryController(AppDbContext context)
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

            var categories = _context.Categories
                .Include(c => c.Products)
                .OrderByDescending(c => c.CategoryId)
                .ToList();

            return View(categories);
        }

        public IActionResult Create()
        {
            var access = CheckAdmin();
            if (access != null) return access;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            if (_context.Categories.Any(c => c.Name == model.Name))
            {
                ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Thêm danh mục thành công.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            if (_context.Categories.Any(c => c.Name == model.Name && c.CategoryId != model.CategoryId))
            {
                ModelState.AddModelError("Name", "Tên danh mục đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId);
                if (category == null) return NotFound();

                category.Name = model.Name;
                _context.SaveChanges();

                TempData["Success"] = "Cập nhật danh mục thành công.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            var category = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var access = CheckAdmin();
            if (access != null) return access;

            var category = _context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null) return NotFound();

            if (category.Products != null && category.Products.Any())
            {
                TempData["Error"] = "Không thể xóa danh mục vì đang có sản phẩm thuộc danh mục này.";
                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            TempData["Success"] = "Xóa danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
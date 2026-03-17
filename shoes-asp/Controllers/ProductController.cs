using Microsoft.AspNetCore.Mvc;
using shoes_asp.Models;

namespace shoes_asp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Women's Boots Shoes Maca", Price = 139, Image = "item-1.jpg" },
                new Product { Id = 2, Name = "Women's Minam Meaghan", Price = 139, Image = "item-2.jpg" },
                new Product { Id = 3, Name = "Men's Taja Commissioner", Price = 139, Image = "item-3.jpg" },
                new Product { Id = 4, Name = "Russ Men's Sneakers", Price = 139, Image = "item-4.jpg" },
                new Product { Id = 5, Name = "Women's Boots Shoes Maca", Price = 139, Image = "item-5.jpg" },
                new Product { Id = 6, Name = "Women's Boots Shoes Maca", Price = 139, Image = "item-6.jpg" },
                new Product { Id = 7, Name = "Women's Boots Shoes Maca", Price = 139, Image = "item-7.jpg" },
                new Product { Id = 8, Name = "Women's Boots Shoes Maca", Price = 139, Image = "item-8.jpg" }
            };

            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = new Product
            {
                Id = id,
                Name = "Women's Boots Shoes Maca",
                Price = 68,
                Image = "item-1.jpg",
                Description = "Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic life."
            };

            return View(product);
        }
    }
}
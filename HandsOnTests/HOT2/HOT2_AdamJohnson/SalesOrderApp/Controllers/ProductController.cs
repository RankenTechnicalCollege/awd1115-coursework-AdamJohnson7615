using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Models;

namespace SalesOrderApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly SalesOrderContext _context;

        public ProductController(SalesOrderContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var products = await _context.Products
                                         .Include(p => p.Category)
                                         .OrderBy(p => p.ProductName)
                                         .ToListAsync();
            return View(products);
        }

        public IActionResult AddEdit(int id = 0)
        {
            ViewBag.Categories = _context.Categories.ToList();

            if (id == 0)
            {
                return View(new Product());
            }
            else
            {
                var product = _context.Products.Find(id);
                if (product == null) return NotFound();
                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEdit(Product product)
        {
            bool categoryTaken = _context.Products
                .Any(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID);

            if (categoryTaken)
            {
                ModelState.AddModelError("CategoryID", "This category is already assigned to another product. Please choose a different category.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }

            if (product.ProductID == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                _context.Products.Update(product);
            }

            _context.SaveChanges();
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.ProductID == id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("List");
        }
    }
}

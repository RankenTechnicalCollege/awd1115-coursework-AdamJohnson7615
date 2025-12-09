using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlushShop.Data;
using PlushShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace PlushShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _ctx;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext ctx, IWebHostEnvironment env)
        {
            _ctx = ctx;
            _env = env;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var products = await _ctx.Products.ToListAsync();
            return View(products);
        }

        // GET: Admin/Products/Create
        public IActionResult Create() => View();

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid) return View(product);

            // Generate slug
            product.Slug = product.Name.ToLower().Replace(" ", "-");

            // Handle image upload
            if (product.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + "_" + product.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await product.ImageFile.CopyToAsync(fileStream);

                product.ImageFileName = uniqueFileName;
            }
            else
            {
                product.ImageFileName = "placeholder.png";
            }

            _ctx.Products.Add(product);
            await _ctx.SaveChangesAsync();
            TempData["Message"] = $"{product.Name} added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _ctx.Products.FindAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return BadRequest();

            var existingProduct = await _ctx.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct == null) return NotFound();
            if (!ModelState.IsValid) return View(product);

            // Generate slug
            product.Slug = product.Name.ToLower().Replace(" ", "-");

            // Handle image upload
            if (product.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid() + "_" + product.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                await product.ImageFile.CopyToAsync(fileStream);

                // Delete old image if not placeholder
                if (!string.IsNullOrEmpty(existingProduct.ImageFileName) && existingProduct.ImageFileName != "placeholder.png")
                {
                    var oldImagePath = Path.Combine(uploadsFolder, existingProduct.ImageFileName);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                product.ImageFileName = uniqueFileName;
            }
            else
            {
                product.ImageFileName = existingProduct.ImageFileName;
            }

            try
            {
                _ctx.Update(product);
                await _ctx.SaveChangesAsync();
                TempData["Message"] = $"{product.Name} updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ctx.Products.Any(e => e.Id == product.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _ctx.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _ctx.Products.FindAsync(id);
            if (product != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");

                // Delete image if not placeholder
                if (!string.IsNullOrEmpty(product.ImageFileName) && product.ImageFileName != "placeholder.png")
                {
                    string imagePath = Path.Combine(uploadsFolder, product.ImageFileName);
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                _ctx.Products.Remove(product);
                await _ctx.SaveChangesAsync();
                TempData["Message"] = $"{product.Name} deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

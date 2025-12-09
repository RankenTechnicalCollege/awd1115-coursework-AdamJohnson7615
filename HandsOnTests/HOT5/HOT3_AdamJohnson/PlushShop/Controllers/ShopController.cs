using Microsoft.AspNetCore.Mvc;
using PlushShop.Data;
using PlushShop.Models;
using Microsoft.EntityFrameworkCore;

namespace PlushShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _ctx;
        public ShopController(AppDbContext ctx) => _ctx = ctx;

        // Home/List all products. Filter by series or company passed in query string.
        [HttpGet("/")]
        public async Task<IActionResult> Index(string? series, string? company)
        {
            var q = _ctx.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(series)) q = q.Where(p => p.Series == series);
            if (!string.IsNullOrWhiteSpace(company)) q = q.Where(p => p.Company == company);

            var products = await q.OrderBy(p => p.Name).ToListAsync();
            ViewBag.SelectedSeries = series;
            ViewBag.SelectedCompany = company;
            return View(products);
        }

        // Details page for a product (display card + picture)
        [HttpGet("products/{slug}/")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();
            var p = await _ctx.Products.FirstOrDefaultAsync(x => x.Slug == slug);
            if (p == null) return NotFound();
            return View(p);
        }
    }
}

using DocStop.Data;
using DocStop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocStop.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _db;
        public CustomersController(AppDbContext db) => _db = db;

        // GET: /Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _db.Customers.AsNoTracking().ToListAsync();
            return View(customers);
        }

        // GET: /Customers/Create
        public IActionResult Create() => View();

        // POST: /Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuarterlySales.Data;
using QuarterlySales.Models;
using System.Linq;

namespace QuarterlySales.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sales
        public IActionResult Index(int? employeeId)
        {
            var sales = _context.Sales.Include(s => s.Employee).AsQueryable();

            if (employeeId.HasValue && employeeId > 0)
            {
                sales = sales.Where(s => s.EmployeeId == employeeId);
            }

            ViewBag.Employees = _context.Employees.ToList();
            return View(sales.ToList());
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        // POST: Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sales sales)
        {
            if (ModelState.IsValid)
            {
                _context.Sales.Add(sales);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = _context.Employees.ToList();
            return View(sales);
        }
    }
}

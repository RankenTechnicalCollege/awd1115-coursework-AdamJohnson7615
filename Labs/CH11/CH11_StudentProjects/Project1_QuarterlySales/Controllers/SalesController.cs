using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project1_QuarterlySales.Data;
using Project1_QuarterlySales.Models;
using System.Linq;

namespace Project1_QuarterlySales.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Sales/
        public IActionResult Index(int? employeeId)
        {
            var employees = _context.Employees
                .OrderBy(e => e.LastName)
                .ToList();

            ViewBag.EmployeeList = new SelectList(employees, "EmployeeId", "FullName", employeeId);

            var sales = _context.Sales
                .Include(s => s.Employee)
                .AsQueryable();

            if (employeeId.HasValue)
            {
                sales = sales.Where(s => s.EmployeeId == employeeId.Value);
            }

            var salesList = sales
                .OrderByDescending(s => s.Year)
                .ThenBy(s => s.Quarter)
                .ToList();

            return View(salesList);
        }

        // GET: /Sales/Create
        public IActionResult Create()
        {
            ViewBag.EmployeeList = new SelectList(_context.Employees, "EmployeeId", "FullName");
            return View();
        }

        // POST: /Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sales sale)
        {
            // Validate duplicate quarter/year/employee
            bool duplicate = _context.Sales.Any(s =>
                s.EmployeeId == sale.EmployeeId &&
                s.Quarter == sale.Quarter &&
                s.Year == sale.Year);

            if (duplicate)
            {
                ModelState.AddModelError("", "This employee already has sales data for the selected quarter and year.");
            }

            if (ModelState.IsValid)
            {
                _context.Sales.Add(sale);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeList = new SelectList(_context.Employees, "EmployeeId", "FullName", sale.EmployeeId);
            return View(sale);
        }
    }
}

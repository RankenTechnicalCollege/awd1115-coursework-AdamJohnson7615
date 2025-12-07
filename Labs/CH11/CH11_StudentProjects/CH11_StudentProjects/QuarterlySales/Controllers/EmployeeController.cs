using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuarterlySales.Data;
using QuarterlySales.Models;
using System.Linq;

namespace QuarterlySales.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public IActionResult Index()
        {
            var employees = _context.Employees.Include(e => e.Manager).ToList();
            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewBag.Managers = _context.Employees.ToList();
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Managers = _context.Employees.ToList();
            return View(employee);
        }
    }
}

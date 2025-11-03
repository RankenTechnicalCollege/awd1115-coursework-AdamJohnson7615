using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project1_QuarterlySales.Data;
using Project1_QuarterlySales.Models;
using System.Linq;

namespace Project1_QuarterlySales.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Employee/Create
        public IActionResult Create()
        {
            ViewBag.ManagerList = new SelectList(_context.Employees, "EmployeeId", "FullName");
            return View();
        }

        // POST: /Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            // Check if employee already exists (same name + DOB)
            bool duplicate = _context.Employees.Any(e =>
                e.FirstName == employee.FirstName &&
                e.LastName == employee.LastName &&
                e.DateOfBirth == employee.DateOfBirth);

            if (duplicate)
            {
                ModelState.AddModelError("", "An employee with the same name and date of birth already exists.");
            }

            // Employee cannot be their own manager
            if (employee.ManagerId == employee.EmployeeId)
            {
                ModelState.AddModelError("", "Employee cannot be their own manager.");
            }

            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("Index", "Sales");
            }

            ViewBag.ManagerList = new SelectList(_context.Employees, "EmployeeId", "FullName", employee.ManagerId);
            return View(employee);
        }
    }
}

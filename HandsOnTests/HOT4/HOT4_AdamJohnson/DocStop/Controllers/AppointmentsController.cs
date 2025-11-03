using DocStop.Data;
using DocStop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace DocStop.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _db;
        public AppointmentsController(AppDbContext db) => _db = db;

        // GET: /Appointments
        public async Task<IActionResult> Index()
        {
            var list = await _db.Appointments.Include(a => a.Customer)
                            .OrderBy(a => a.Start)
                            .AsNoTracking()
                            .ToListAsync();
            return View(list);
        }

        // GET: /Appointments/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CustomerId = new SelectList(await _db.Customers.AsNoTracking().ToListAsync(), "Id", "Username");
            return View();
        }

        // POST: /Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CustomerId = new SelectList(await _db.Customers.AsNoTracking().ToListAsync(), "Id", "Username", appointment.CustomerId);
                return View(appointment);
            }

            _db.Appointments.Add(appointment);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(nameof(appointment.Start),
                    $"The appointment slot ({appointment.Start:yyyy-MM-dd HH:mm}) is already taken. Please choose another hour.");
                ViewBag.CustomerId = new SelectList(await _db.Customers.AsNoTracking().ToListAsync(), "Id", "Username", appointment.CustomerId);
                return View(appointment);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: /Appointments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _db.Appointments.Include(a => a.Customer)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }
    }
}

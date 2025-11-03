using DocStop.Data;
using DocStop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocStop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Only admins should be able to access edit/delete. Remove if not using auth yet.
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _db;
        public AppointmentsController(AppDbContext db) => _db = db;

        // GET: /Admin/Appointments
        public async Task<IActionResult> Index()
        {
            var list = await _db.Appointments.Include(a => a.Customer).OrderBy(a => a.Start).ToListAsync();
            return View(list);
        }

        // GET: /Admin/Appointments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var appt = await _db.Appointments.FindAsync(id);
            if (appt == null) return NotFound();

            ViewBag.Customers = await _db.Customers.AsNoTracking().ToListAsync();
            return View(appt);
        }

        // POST: /Admin/Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _db.Customers.AsNoTracking().ToListAsync();
                return View(appointment);
            }

            _db.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(nameof(appointment.Start),
                    $"The appointment slot ({appointment.Start:yyyy-MM-dd HH:mm}) is already taken. Please choose another hour.");
                ViewBag.Customers = await _db.Customers.AsNoTracking().ToListAsync();
                return View(appointment);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Appointments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var appt = await _db.Appointments.Include(a => a.Customer).FirstOrDefaultAsync(a => a.Id == id);
            if (appt == null) return NotFound();
            return View(appt);
        }

        // POST: /Admin/Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appt = await _db.Appointments.FindAsync(id);
            if (appt != null)
            {
                _db.Appointments.Remove(appt);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Appointments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var appt = await _db.Appointments.Include(a => a.Customer).FirstOrDefaultAsync(a => a.Id == id);
            if (appt == null) return NotFound();
            return View(appt);
        }
    }
}

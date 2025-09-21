using Microsoft.AspNetCore.Mvc;
using Project1_ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace Project1_ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ContactContext _context;

        public ContactsController(ContactContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts
                                         .Include(c => c.Category)
                                         .ToListAsync();
            return View(contacts);
        }

        public async Task<IActionResult> Details(int id, string slug)
        {
            var contact = await _context.Contacts
                                        .Include(c => c.Category)
                                        .FirstOrDefaultAsync(c => c.ContactId == id);
            if (contact == null) return NotFound();

            if (!string.Equals(slug, contact.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Details", new { id = id, slug = contact.Slug });
            }

            return View(contact);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View("Edit", new Contact());
        }

        public IActionResult Edit(int id, string slug)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null) return NotFound();

            if (!string.IsNullOrEmpty(slug) && !string.Equals(slug, contact.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Edit", new { id = id, slug = contact.Slug });
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(contact);
            }

            if (contact.ContactId == 0)
            {
                contact.DateAdded = DateTime.Now;
                _context.Contacts.Add(contact);
            }
            else
            {
                _context.Contacts.Update(contact);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id, string slug)
        {
            var contact = await _context.Contacts
                                        .Include(c => c.Category)
                                        .FirstOrDefaultAsync(c => c.ContactId == id);
            if (contact == null) return NotFound();

            if (!string.IsNullOrEmpty(slug) && !string.Equals(slug, contact.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Delete", new { id = id, slug = contact.Slug });
            }

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace StarLogger.Controllers
{
    public class ContactController : Controller
    {
        // GET: /Contact
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string name, string email, string message)
        {

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Please fill out all fields before sending.";
                return View();
            }

            TempData["Success"] = "Message received! We will review your inquiry shortly.";
            return RedirectToAction("Index");
        }
    }
}
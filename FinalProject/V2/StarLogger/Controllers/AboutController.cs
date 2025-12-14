using Microsoft.AspNetCore.Mvc;

namespace StarLogger.Controllers
{
    public class AboutController : Controller
    {
        // GET: /About
        public IActionResult Index()
        {
            return View();
        }
    }
}
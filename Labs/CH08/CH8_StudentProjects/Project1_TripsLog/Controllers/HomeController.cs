using Microsoft.AspNetCore.Mvc;
using Project1_TripsLog.Data;
using Project1_TripsLog.Models;

namespace Project1_TripsLog.Controllers
{
    public class HomeController : Controller
    {
        private readonly TripsContext _context;

        public HomeController(TripsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Trips Log";
            ViewBag.SubHeader = null; // none for home page
            ViewBag.Message = TempData["Message"] as string;

            var trips = _context.Trips.ToList();
            return View(trips);
        }
    }
}
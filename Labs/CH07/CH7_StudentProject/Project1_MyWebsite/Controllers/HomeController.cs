using Microsoft.AspNetCore.Mvc;

namespace Project1_MyWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "About";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact";

            var contacts = new List<(string Label, string Value)>
            {
                ("Phone", "111-111-1111"),
                ("Email", "adam_johnson@insideranken.org"),
                ("Facebook", "https://facebook.com/adamjohnson")
            };

            return View(contacts);
        }
    }
}
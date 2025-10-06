using Microsoft.AspNetCore.Mvc;

namespace Project1_MyWebsite.Areas.Help.Controllers
{
    [Area("Help")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Help";
            return View();
        }
    }
}
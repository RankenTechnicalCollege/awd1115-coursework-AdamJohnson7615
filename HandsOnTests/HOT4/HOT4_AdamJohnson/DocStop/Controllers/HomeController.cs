using Microsoft.AspNetCore.Mvc;

namespace DocStop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About() => View();
    }
}

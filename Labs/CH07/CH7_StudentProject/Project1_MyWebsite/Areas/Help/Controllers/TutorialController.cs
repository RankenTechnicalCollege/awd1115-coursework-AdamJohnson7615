using Microsoft.AspNetCore.Mvc;

namespace Project1_MyWebsite.Areas.Help.Controllers
{
    [Area("Help")]
    public class TutorialController : Controller
    {
        public IActionResult Page1()
        {
            return View();
        }

        public IActionResult Page2()
        {
            return View();
        }

        public IActionResult Page3()
        {
            return View();
        }
    }
}

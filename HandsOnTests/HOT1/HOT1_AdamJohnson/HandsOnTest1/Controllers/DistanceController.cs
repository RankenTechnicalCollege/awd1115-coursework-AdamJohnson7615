using Microsoft.AspNetCore.Mvc;
using HandsOnTest1.Models;

namespace HandsOnTest1.Controllers
{
    public class DistanceController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new DistanceConverter());
        }

        [HttpPost]
        public IActionResult Index(DistanceConverter model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                model.Inches = 0;
                return View(model);
            }
        }
    }
}

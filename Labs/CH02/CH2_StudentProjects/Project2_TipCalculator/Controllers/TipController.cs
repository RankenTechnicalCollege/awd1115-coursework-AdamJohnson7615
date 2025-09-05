using Microsoft.AspNetCore.Mvc;
using Project2_TipCalculator.Models;

namespace Project2_TipCalculator.Controllers
{
    public class TipController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new TipCalculator();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(TipCalculator model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                model.MealCost = 0;
                return View(model);
            }
        }

        public IActionResult Clear()
        {
            var model = new TipCalculator();
            return View("Index", model);
        }
    }
}

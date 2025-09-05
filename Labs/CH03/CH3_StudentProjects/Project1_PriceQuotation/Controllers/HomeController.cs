using Microsoft.AspNetCore.Mvc;
using Project1_PriceQuotation.Models;

namespace Project1_PriceQuotation.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new PriceQuotation();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PriceQuotation model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult Clear()
        {
            var model = new PriceQuotation();
            return View("Index", model);
        }
    }
}

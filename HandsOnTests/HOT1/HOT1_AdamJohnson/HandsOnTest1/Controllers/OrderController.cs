using Microsoft.AspNetCore.Mvc;
using HandsOnTest1.Models;

namespace HandsOnTest1.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new OrderForm());
        }

        [HttpPost]
        public IActionResult Index(OrderForm model)
        {
            if (ModelState.IsValid)
            {
                model.ApplyDiscountCode();
                return View(model);
            }
            return View(model);
        }
    }
}

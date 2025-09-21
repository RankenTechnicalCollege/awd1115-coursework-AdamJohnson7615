using Microsoft.AspNetCore.Mvc;

namespace SalesOrderApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

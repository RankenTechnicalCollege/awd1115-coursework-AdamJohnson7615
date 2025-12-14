using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StarLogger.Data;
using StarLogger.Models;
using System.Diagnostics;

namespace StarLogger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // GET: / (The Home Page / Feed)
        public async Task<IActionResult> Index()
        {

            ViewBag.LastViewedName = HttpContext.Session.GetString("LastViewedGame");
            ViewBag.LastViewedId = HttpContext.Session.GetInt32("LastViewedGameId");

            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Game)
                .OrderByDescending(p => p.DatePosted) 
                .Take(20) 
                .ToListAsync();

            var userId = _userManager.GetUserId(User);

            // Handle guest users safely (userId will be null if not logged in)
            if (userId != null)
            {
                ViewBag.LikedPostIds = await _context.PostLikes
                    .Where(pl => pl.UserId == userId)
                    .Select(pl => pl.PostId)
                    .ToListAsync();
            }
            else
            {
                ViewBag.LikedPostIds = new List<int>();
            }

            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
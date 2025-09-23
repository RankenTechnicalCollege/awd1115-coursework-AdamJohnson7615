using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;
using System.Threading.Tasks;

namespace StarLogger.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly StarLoggerContext _context;

        public AchievementsController(StarLoggerContext context)
        {
            _context = context;
        }

        // GET: Achievements
        public async Task<IActionResult> Index()
        {
            var achievements = await _context.Achievements
                .Include(a => a.Game)
                .ToListAsync();
            return View(achievements);
        }

        // GET: Achievements/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title");
            return View();
        }

        // POST: Achievements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Achievement achievement)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
                return View(achievement);
            }

            _context.Add(achievement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

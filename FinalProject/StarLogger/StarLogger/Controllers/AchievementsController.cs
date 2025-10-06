using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Helpers;
using StarLogger.Models;
using System.Linq;
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

        // GET: /achievements/
        public async Task<IActionResult> Index()
        {
            var achievements = await _context.Achievements
                .Include(a => a.Game)
                .ToListAsync();

            ViewData["Title"] = "Achievements";
            return View(achievements);
        }

        // GET: /achievements/{id}/{slug}/
        public async Task<IActionResult> Details(int? id, string slug)
        {
            if (id == null) return NotFound();

            var achievement = await _context.Achievements
                .Include(a => a.Game)
                .FirstOrDefaultAsync(a => a.AchievementId == id);

            if (achievement == null) return NotFound();

            ViewData["Title"] = achievement.Name;

            var generatedSlug = SlugHelper.ToSlug(achievement.Name);
            if (slug != generatedSlug)
                return RedirectToAction("Details", new { id, slug = generatedSlug });

            return View(achievement);
        }
    }
}

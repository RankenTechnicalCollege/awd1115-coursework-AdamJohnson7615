using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;
using StarLogger.Helpers;
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

        // GET: /achievements/create/
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title");
            ViewData["Title"] = "Create Achievement";
            return View();
        }

        // POST: /achievements/create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Achievement achievement)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
                ViewData["Title"] = "Create Achievement";
                return View(achievement);
            }

            _context.Add(achievement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /achievements/edit/{id}/{slug}/
        public async Task<IActionResult> Edit(int? id, string slug)
        {
            if (id == null) return NotFound();

            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null) return NotFound();

            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
            ViewData["Title"] = $"Edit {achievement.Name}";
            return View(achievement);
        }

        // POST: /achievements/edit/{id}/{slug}/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Achievement achievement)
        {
            if (id != achievement.AchievementId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
                ViewData["Title"] = $"Edit {achievement.Name}";
                return View(achievement);
            }

            try
            {
                _context.Update(achievement);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Achievements.Any(a => a.AchievementId == id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /achievements/delete/{id}/{slug}/
        public async Task<IActionResult> Delete(int? id, string slug)
        {
            if (id == null) return NotFound();

            var achievement = await _context.Achievements
                .Include(a => a.Game)
                .FirstOrDefaultAsync(a => a.AchievementId == id);

            if (achievement == null) return NotFound();

            ViewData["Title"] = $"Delete {achievement.Name}";
            return View(achievement);
        }

        // POST: /achievements/delete/{id}/{slug}/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement != null)
            {
                _context.Achievements.Remove(achievement);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

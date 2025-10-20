using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: /achievements/create/
        public IActionResult Create()
        {
            // Populate the Game dropdown (SelectList) with explicit projection to ensure correct value type
            ViewData["GameId"] = new SelectList(
                _context.Games
                    .OrderBy(g => g.Title)
                    .Select(g => new { g.GameId, g.Title })
                    .ToList(),
                "GameId", "Title"
            );
            return View();
        }

        // POST: /achievements/create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Achievement achievement)
        {
            // Debugging: log received GameId
            Console.WriteLine("Received GameId: " + achievement.GameId);

            if (ModelState.IsValid)
            {
                _context.Add(achievement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate the Game dropdown if validation fails
            ViewData["GameId"] = new SelectList(
                _context.Games
                    .OrderBy(g => g.Title)
                    .Select(g => new { g.GameId, g.Title })
                    .ToList(),
                "GameId", "Title", achievement.GameId
            );
            return View(achievement);
        }

        // GET: /achievements/edit/{id}/{slug}/
        public async Task<IActionResult> Edit(int? id, string slug)
        {
            if (id == null) return NotFound();

            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null) return NotFound();

            ViewData["Title"] = "Edit Achievement";
            ViewData["GameId"] = new SelectList(
                _context.Games
                    .OrderBy(g => g.Title)
                    .Select(g => new { g.GameId, g.Title })
                    .ToList(),
                "GameId", "Title", achievement.GameId
            );

            var generatedSlug = SlugHelper.ToSlug(achievement.Name);
            if (!string.IsNullOrEmpty(slug) && slug != generatedSlug)
                return RedirectToAction("Edit", new { id, slug = generatedSlug });

            return View(achievement);
        }

        // POST: /achievements/edit/{id}/{slug}/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Achievement achievement)
        {
            if (id != achievement.AchievementId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(achievement);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AchievementExists(achievement.AchievementId))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Repopulate dropdown if validation fails
            ViewData["GameId"] = new SelectList(
                _context.Games
                    .OrderBy(g => g.Title)
                    .Select(g => new { g.GameId, g.Title })
                    .ToList(),
                "GameId", "Title", achievement.GameId
            );
            return View(achievement);
        }

        // GET: /achievements/delete/{id}/{slug}/
        public async Task<IActionResult> Delete(int? id, string slug)
        {
            if (id == null) return NotFound();

            var achievement = await _context.Achievements
                .Include(a => a.Game)
                .FirstOrDefaultAsync(a => a.AchievementId == id);

            if (achievement == null) return NotFound();

            ViewData["Title"] = "Delete Achievement";
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

        private bool AchievementExists(int id)
        {
            return _context.Achievements.Any(e => e.AchievementId == id);
        }
    }
}
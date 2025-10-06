using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StarLogger.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageAchievementsController : Controller
    {
        private readonly StarLoggerContext _context;

        public ManageAchievementsController(StarLoggerContext context)
        {
            _context = context;
        }

        // GET: /manage/achievements/create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title");
            return View();
        }

        // POST: /manage/achievements/create
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
            return RedirectToAction("Index", "Achievements");
        }

        // GET: /manage/achievements/edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null) return NotFound();

            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
            return View(achievement);
        }

        // POST: /manage/achievements/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Achievement achievement)
        {
            if (id != achievement.AchievementId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
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

            return RedirectToAction("Index", "Achievements");
        }

        // GET: /manage/achievements/delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var achievement = await _context.Achievements
                .Include(a => a.Game)
                .FirstOrDefaultAsync(a => a.AchievementId == id);

            if (achievement == null) return NotFound();

            return View(achievement);
        }

        // POST: /manage/achievements/delete/{id}
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
            return RedirectToAction("Index", "Achievements");
        }
    }
}

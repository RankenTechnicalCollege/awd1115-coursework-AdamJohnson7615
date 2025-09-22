using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StarLogger.Data;
using StarLogger.Models;
using System.Linq;

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
        public IActionResult Index()
        {
            var achievements = _context.Achievements
                .Select(a => new Achievement
                {
                    AchievementId = a.AchievementId,
                    Name = a.Name,
                    GameId = a.GameId,
                    Points = a.Points,
                    Game = a.Game
                }).ToList();

            return View(achievements);
        }

        // GET: Achievements/Create
        public IActionResult Create()
        {
            ViewBag.GameId = new SelectList(_context.Games, "GameId", "Title");
            return View();
        }

        // POST: Achievements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Achievement achievement)
        {
            if (ModelState.IsValid)
            {
                _context.Achievements.Add(achievement);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GameId = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
            return View(achievement);
        }

        // GET: Achievements/Edit/5
        public IActionResult Edit(int id)
        {
            var achievement = _context.Achievements.Find(id);
            if (achievement == null) return NotFound();

            ViewBag.GameId = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
            return View(achievement);
        }

        // POST: Achievements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Achievement achievement)
        {
            if (id != achievement.AchievementId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(achievement);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GameId = new SelectList(_context.Games, "GameId", "Title", achievement.GameId);
            return View(achievement);
        }

        // GET: Achievements/Delete/5
        public IActionResult Delete(int id)
        {
            var achievement = _context.Achievements.FirstOrDefault(a => a.AchievementId == id);
            if (achievement == null) return NotFound();
            return View(achievement);
        }

        // POST: Achievements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var achievement = _context.Achievements.Find(id);
            if (achievement != null)
            {
                _context.Achievements.Remove(achievement);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
